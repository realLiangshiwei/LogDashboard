using System;
using Microsoft.AspNetCore.Http;
using NLogDashboard.Route;
using RazorLight;

namespace NLogDashboard
{
    public class NLogDashboardContext
    {
        public HttpContext HttpContext { get; }

        public NLogDashboardRoute Route { get; }

        public IRazorLightEngine Engine { get; }

        public NLogDashboardOptions Options { get; }

        public NLogDashboardContext(HttpContext httpContext, NLogDashboardRoute route, IRazorLightEngine engine, NLogDashboardOptions options)
        {
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Options = options;
        }
    }
}
