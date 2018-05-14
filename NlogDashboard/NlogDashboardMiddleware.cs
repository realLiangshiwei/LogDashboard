using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
         
            var router = NlogDashboardRoutes.Routes.FindRoute(httpContext.Request.Path.Value);

            if (router == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }


            var handle = Assembly.GetAssembly(typeof(NlogDashboardRoute))
                .CreateInstance(router.Handle+"Handle", true, BindingFlags.CreateInstance, null, new object[]
                {
                    new NlogDashboardContext(httpContext, router,
                        httpContext.RequestServices.GetService(typeof(IRazorLightEngine)) as IRazorLightEngine)
                }, null, null);



            var html = await (Task<string>)handle.GetType().GetMethod(router.Action).Invoke(handle, null);


            await httpContext.Response.WriteAsync(html);

        }
    }
}
