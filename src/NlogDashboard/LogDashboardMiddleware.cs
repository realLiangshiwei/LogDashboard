using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using LogDashboard.Authorization;
using LogDashboard.EmbeddedFiles;
using LogDashboard.Handle;
using LogDashboard.Route;
using RazorLight;

namespace LogDashboard
{
    public class LogDashboardMiddleware
    {
        private readonly RequestDelegate _next;


        public LogDashboardMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            var opts = httpContext.RequestServices.GetService<LogDashboardOptions>();

            var requestUrl = httpContext.Request.Path.Value;

            //EmbeddedFile
            if (requestUrl.Contains("css") || requestUrl.Contains("js"))
            {
                await httpContext.Response.WriteAsync(LogDashboardEmbeddedFiles.IncludeEmbeddedFile(requestUrl));
                return;
            }

            // Find Router
            var router = LogDashboardRoutes.Routes.FindRoute(requestUrl);

            if (router == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }


            //Authorization
            if (!await AuthorizeHelper.AuthorizeAsync(httpContext, opts.AuthorizeData))
            {
                return;
            }

            var logDashboardContext = new LogDashboardContext(httpContext, router,
                httpContext.RequestServices.GetService<IRazorLightEngine>(),
                opts);

            if (!AuthorizationFilterHelper.Authorization(opts.AuthorizationFiles, logDashboardContext))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }


            //Activate Handle
            var handleType = Assembly.GetAssembly(typeof(LogDashboardRoute))
                .GetTypes().FirstOrDefault(x => x.Name.Contains(router.Handle + "Handle"));

            var handle = httpContext.RequestServices.GetRequiredService(handleType.MakeGenericType(opts.LogModelType)) as ILogDashboardHandle;

            if (handle == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            handle.Context = logDashboardContext;

            string html;

            var method = handle.GetType().GetMethod(router.Action);
            // ReSharper disable once PossibleNullReferenceException
            var parametersLength = method.GetParameters().Length;

            if (parametersLength == 0)
            {
                html = await (Task<string>)method.Invoke(handle, null);
            }
            else
            {
                if (httpContext.Request.ContentLength == null && httpContext.Request.Query.Count <= 0)
                {
                    html = await (Task<string>)method.Invoke(handle, new Object[] { null });
                }
                else
                {
                    object args;
                    if (httpContext.Request.Query.Count > 0)
                    {
                        var dict = new Dictionary<string, string>();
                        httpContext.Request.Query.ToList().ForEach(x => dict.Add(x.Key, x.Value));
                        args = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dict), method.GetParameters().First().ParameterType);
                    }
                    else
                    {
                        // ReSharper disable once PossibleInvalidOperationException
                        var bytes = new byte[(int)httpContext.Request.ContentLength];
                        await httpContext.Request.Body.ReadAsync(bytes, 0, (int)httpContext.Request.ContentLength);
                        string requestJson = Encoding.Default.GetString(bytes);

                        args = JsonConvert.DeserializeObject(requestJson, method.GetParameters().First().ParameterType);

                    }

                    html = await (Task<string>)method.Invoke(handle, new[] { args });

                }
            }

            await httpContext.Response.WriteAsync(html);


        }
    }
}
