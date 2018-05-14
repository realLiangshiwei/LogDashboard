using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
                    var cred = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
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

            var conn = httpContext.RequestServices.GetService<SqlConnection>();
            await conn.OpenAsync();

            var handle = Assembly.GetAssembly(typeof(NlogDashboardRoute))
                .CreateInstance("NlogDashboard.Handle." + router.Handle + "Handle", true, BindingFlags.Default, null, new object[]
                {
                    new NlogDashboardContext(httpContext, router,
                        httpContext.RequestServices.GetService<IRazorLightEngine>()),
                    conn
                }, null, null);


            if (handle == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            var html = await (Task<string>)handle.GetType().GetMethod(router.Action).Invoke(handle, null);

            await httpContext.Response.WriteAsync(html);
            conn.Close();
        }
    }
}
