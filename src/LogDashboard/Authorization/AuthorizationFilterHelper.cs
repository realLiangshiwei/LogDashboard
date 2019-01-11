using System.Collections.Generic;
using System.Linq;

namespace LogDashboard.Authorization
{
    public class AuthorizationFilterHelper
    {
        public static bool Authorization(List<ILogDashboardAuthorizationFilter> filters, LogDashboardContext context)
        {
            if (filters.Count == 0)
                return true;
            return filters.All(x => x.Authorization(context));
        }
    }
}
