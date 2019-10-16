using System;
using Microsoft.AspNetCore.Http;
using LogDashboard.Route;
using RazorLight;

namespace LogDashboard
{
    public class LogDashboardContext
    {

        public HttpContext HttpContext { get; }

        public LogDashboardRoute Route { get; }

        public IRazorLightEngine Engine { get; }

        public LogDashboardOptions Options { get; }


        public static LogDashboardOptions StaticOptions { get; set; }


        public LogDashboardContext(HttpContext httpContext, LogDashboardRoute route, IRazorLightEngine engine, LogDashboardOptions options)
        {
            StaticOptions = options;
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Options = options;
        }
    }
}
