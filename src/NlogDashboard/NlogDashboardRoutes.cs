using System;
using System.Collections.Generic;
using System.Text;

namespace NlogDashboard
{
    public static class NlogDashboardRoutes
    {
        public static RouteCollection Routes { get; }

        static NlogDashboardRoutes()
        {
            Routes = new RouteCollection();
            Routes.AddRoute(new NlogDashboardRoute()
            {
                Key = "Dashboard/Home",
                View = "Views.Dashboard.Home.cshtml"
            });
        }
    }
}
