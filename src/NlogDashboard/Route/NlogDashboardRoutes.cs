namespace NLogDashboard.Route
{
    public static class NLogDashboardRoutes
    {
        public static RouteCollection Routes { get; }

        static NLogDashboardRoutes()
        {
            Routes = new RouteCollection();

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/Home", "Views.Dashboard.Home.cshtml"));

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/SearchLog", "Views.Dashboard.LogList.cshtml"));

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/BasicLog", "Views.Dashboard.BasicLog.cshtml"));

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/ErrorLog", "Views.Dashboard.LogList.cshtml"));

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/LogInfo", "Views.Dashboard.LogInfo.cshtml"));

            Routes.AddRoute(new NLogDashboardRoute
            {
                Key = "/Dashboard/GetException",
                HtmlView = false
            });

            Routes.AddRoute(new NLogDashboardRoute("/Dashboard/Ha", "Views.Dashboard.Exception.cshtml"));
           
        }
    }
}
