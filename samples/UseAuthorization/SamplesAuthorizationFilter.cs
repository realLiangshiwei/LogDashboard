using LogDashboard;
using LogDashboard.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace UseAuthorization
{
    public class SamplesAuthorizationFilter : ILogDashboardAuthorizationFilter
    {

        public bool Authorization(LogDashboardContext context)
        {
            var url = context.HttpContext.Request.GetDisplayUrl();
            return url.Contains("localhost") || url.Contains("127.0.0.1");
        }
    }
}
