using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLogDashboard.Authorization;
using NLogDashboard.Route;
using RazorLight;

namespace NLogDashboard
{
    public class NLogDashboardMiddleware
    {
        private readonly RequestDelegate _next;


        public NLogDashboardMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            var opts = httpContext.RequestServices.GetService<NLogDashboardOptions>();

            if (opts.Authorization)
            {
                if (!await AuthorizeHelper.AuthorizeAsync(httpContext, opts.AuthorizeData))
                {
                    return;
                }
            }


            var requestUrl = httpContext.Request.Path.Value;

            if (requestUrl.Contains("css") || requestUrl.Contains("js"))
            {
                await httpContext.Response.WriteAsync(NLogDashboardEmbeddedFiles.IncludeEmbeddedFile(requestUrl));
                return;
            }


            var router = NLogDashboardRoutes.Routes.FindRoute(requestUrl);

            if (router == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            var list = Assembly.GetAssembly(typeof(NLogDashboardRoute)).GetType(router.Handle + "Handle");

            var handle = Assembly.GetAssembly(typeof(NLogDashboardRoute))
                .CreateInstance("NLogDashboard.Handle." + router.Handle + "Handle", true, BindingFlags.Default, null, new object[]
                {
                        new NLogDashboardContext(httpContext, router,
                            httpContext.RequestServices.GetService<IRazorLightEngine>(),
                            opts),

                        httpContext.RequestServices

                }, null, null);


            if (handle == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            string html;

            var method = handle.GetType().GetMethod(router.Action);
            // ReSharper disable once PossibleNullReferenceException
            var parameterslength = method.GetParameters().Length;

            if (parameterslength == 0)
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
