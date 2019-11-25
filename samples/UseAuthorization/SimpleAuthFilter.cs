using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogDashboard;
using LogDashboard.Authorization;

namespace UseAuthorization
{
    public class SimpleAuthFilter : ILogDashboardAuthorizationFilter
    {
        public bool Authorization(LogDashboardContext context)
        {
            return context.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
