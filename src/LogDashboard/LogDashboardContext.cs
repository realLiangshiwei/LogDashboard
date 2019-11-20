using System;
using Microsoft.AspNetCore.Http;
using LogDashboard.Route;

namespace LogDashboard
{
    public class LogDashboardContext
    {

        public HttpContext HttpContext { get; }

        public LogDashboardRoute Route { get; }

        public LogDashboardOptions Options { get; }


        public static LogDashboardOptions StaticOptions { get; set; }


        public LogDashboardContext(HttpContext httpContext, LogDashboardRoute route, LogDashboardOptions options)
        {
            StaticOptions = options;
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Options = options;
        }
    }
}
