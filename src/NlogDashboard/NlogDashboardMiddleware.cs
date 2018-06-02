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
using RazorLight;

namespace NlogDashboard
{
    public class NlogDashboardMiddleware
    {
        private readonly RequestDelegate _next;

        public NlogDashboardMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            var opts = httpContext.RequestServices.GetService<NlogDashboardOptions>();

            #region auth
            if (opts.UseAuthorzation)
            {
                string auth = httpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrWhiteSpace(auth))
                {
                    var cred = Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                    var user = new { Name = cred[0], Pass = cred[1] };
                    if (user.Name == opts.Name && user.Pass == opts.Password)
                    {
                        goto AuthSuccess;
                    }
                }
                httpContext.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"NlogDashBoard Auth\"");
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("unAuthorization");
                return;
            }
            AuthSuccess:
            #endregion


            var requestUrl = httpContext.Request.Path.Value;
            if (requestUrl.Contains("css") || requestUrl.Contains("js"))
            {
                await httpContext.Response.WriteAsync(NlogDashboardEmbeddedFiles.IncludeEmbeddedFile(requestUrl));
                return;
            }
            var router = NlogDashboardRoutes.Routes.FindRoute(requestUrl);

            if (router == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            using (var conn = httpContext.RequestServices.GetService<SqlConnection>())
            {
                await conn.OpenAsync();
                var handle = Assembly.GetAssembly(typeof(NlogDashboardRoute))
                    .CreateInstance("NlogDashboard.Handle." + router.Handle + "Handle", true, BindingFlags.Default, null, new object[]
                    {
                        new NlogDashboardContext(httpContext, router,
                            httpContext.RequestServices.GetService<IRazorLightEngine>(),
                            opts),
                        conn

                    }, null, null);


                if (handle == null)
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }

                string html;

                var method = handle.GetType().GetMethod(router.Action);
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
}
