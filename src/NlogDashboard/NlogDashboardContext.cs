using System;
using Microsoft.AspNetCore.Http;
using RazorLight;

namespace NlogDashboard
{
    public class NlogDashboardContext
    {
        public HttpContext HttpContext { get; }

        public NlogDashboardRoute Route { get; }

        public IRazorLightEngine Engine { get; }

        public NlogDashboardOptions Options { get; }

        public NlogDashboardContext(HttpContext httpContext, NlogDashboardRoute route, IRazorLightEngine engine, NlogDashboardOptions options)
        {
            Route = route ?? throw new ArgumentNullException(nameof(route));
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Options = options;
        }
    }
}
