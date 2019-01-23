using System;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif
using LogDashboard.Route;
#if NETFRAMEWORK
using Microsoft.Owin;
#endif
using RazorLight;

namespace LogDashboard
{
    public class LogDashboardContext
    {

#if NETSTANDARD2_0
        public HttpContext HttpContext { get; }
#endif

#if NETFRAMEWORK
        public IOwinContext HttpContext { get; set; }
#endif

        public LogDashboardRoute Route { get; }

        public IRazorLightEngine Engine { get; }

        public LogDashboardOptions Options { get; }


        public static LogDashboardOptions StaticOptions { get; set; }


#if NETSTANDARD2_0
        public LogDashboardContext(HttpContext httpContext, LogDashboardRoute route, IRazorLightEngine engine, LogDashboardOptions options)
        {
            StaticOptions = options;
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Options = options;
        }
#endif



#if NETFRAMEWORK
        public LogDashboardContext(IOwinContext httpContext, LogDashboardRoute route, IRazorLightEngine engine, LogDashboardOptions options)
        {
            StaticOptions = options;
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Options = options;
        }
#endif

    }
}
