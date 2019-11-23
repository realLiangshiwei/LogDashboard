using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogDashboard.Authorization.Filters
{
    public class LogDashboardRoleAuthorizeFilter : ILogDashboardAuthorizationFilter
    {
        public List<string> RoleNames { get; set; }

        public LogDashboardRoleAuthorizeFilter(List<string> roleNames)
        {
            RoleNames = roleNames;
        }
        public bool Authorization(LogDashboardContext context)
        {
            return RoleNames.Any(roleName => context.HttpContext.User.IsInRole(roleName));
        }
    }
}
