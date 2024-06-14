using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using LogDashboard.Authorization;
using LogDashboard.EmbeddedFiles;
using LogDashboard.Handle;
using LogDashboard.Repository;
using LogDashboard.Route;
using Microsoft.Extensions.DependencyInjection;

namespace LogDashboard {

    public class LogDashboardMiddleware {
        private readonly RequestDelegate _next;

        public LogDashboardMiddleware(RequestDelegate next) {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext) {
            using var scope = httpContext.RequestServices.CreateScope();
            var opts = scope.ServiceProvider.GetService<LogDashboardOptions>();

            var requestUrl = httpContext.Request.Path.Value;

            //EmbeddedFile
            if (requestUrl.Contains("css") || requestUrl.Contains("js") || requestUrl.Contains("woff")) {
                await LogDashboardEmbeddedFiles.IncludeEmbeddedFile(httpContext, requestUrl);
                return;
            }

            // Find Router
            var router = LogDashboardRoutes.Routes.FindRoute(requestUrl);

            if (router == null) {
                httpContext.Response.StatusCode = 404;
                return;
            }

            // Authorization
            if (!await AuthorizeHelper.AuthorizeAsync(httpContext, opts.AuthorizeData)) {
                return;
            }

            var logDashboardContext = new LogDashboardContext(httpContext, router,
                opts);

            if (!AuthorizationFilterHelper.Authorization(opts.AuthorizationFiles, logDashboardContext)) {
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.OK) {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                return;
            }

            using var uow = scope.ServiceProvider.GetService<IUnitOfWork>();
            await uow.Open();

            //Activate Handle
            var handleType = Assembly.GetAssembly(typeof(LogDashboardRoute))
                .GetTypes().FirstOrDefault(x => x.Name.Contains(router.Handle + "Handle"));

            var handle =
                scope.ServiceProvider.GetRequiredService(handleType.MakeGenericType(opts.LogModelType)) as
                    ILogDashboardHandle;

            if (handle == null) {
                httpContext.Response.StatusCode = 404;
                return;
            }


            handle.Context = logDashboardContext;

            string html;

            var method = handle.GetType().GetMethod(router.Action);
            // ReSharper disable once PossibleNullReferenceException
            var parametersLength = method.GetParameters().Length;

            if (parametersLength == 0) {
                html = await (Task<string>)method.Invoke(handle, null);
            } else {
                if (httpContext.Request.ContentLength == null && httpContext.Request.Query.Count <= 0) {
                    html = await (Task<string>)method.Invoke(handle, new Object[] { null });
                } else {
                    object args;
                    if (httpContext.Request.Query.Count > 0) {
                        var dict = new Dictionary<string, string>();
                        httpContext.Request.Query.ToList().ForEach(x => dict.Add(x.Key, x.Value));
                        args = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dict),
                                            method.GetParameters().First().ParameterType);
                    } else {

                        using var reader = new StreamReader(httpContext.Request.Body);
                        var requestJson = await reader.ReadToEndAsync();

                        args = JsonConvert.DeserializeObject(requestJson,
                        method.GetParameters().First().ParameterType);

                    }

                    html = await (Task<string>)method.Invoke(handle, new[] { args });

                }
            }

            await httpContext.Response.WriteAsync(html);
        }
    }
}
