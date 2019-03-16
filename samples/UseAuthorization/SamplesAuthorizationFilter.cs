using LogDashboard;
using LogDashboard.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace UseAuthorization
{
    public class SamplesAuthorizationFilter : ILogDashboardAuthorizationFilter
    {

        public bool Authorization(LogDashboardContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.Redirect("/Identity/Account/Login?returnUrl=/logdashboard");
                return false;
            }
      
            return true;
        }
    }
}
