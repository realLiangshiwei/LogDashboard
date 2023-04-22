using LogDashboard.Views.Dashboard;

namespace LogDashboard.Route
{
    public static class LogDashboardRoutes
    {
        public static RouteCollection Routes { get; }

        static LogDashboardRoutes()
        {
            Routes = new RouteCollection();

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/Home", typeof(Home)));

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/SearchLog", typeof(LogList)));

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/BasicLog", typeof(BasicLog)));

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/ErrorLog", typeof(LogList)));

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/LogInfo", typeof(LogInfo)));

            Routes.AddRoute(new LogDashboardRoute("/Dashboard/Login", typeof(Login)));

            Routes.AddRoute(new LogDashboardRoute
            {
                Key = "/Dashboard/GetException",
                HtmlView = false
            });

            Routes.AddRoute(new LogDashboardRoute
            {
                Key = "/Dashboard/RequestTrace",
                HtmlView = false
            });

            Routes.AddRoute(new LogDashboardRoute
            {
                Key = "/Dashboard/GetLogChart",
                HtmlView = false
            });

        }
    }
}
