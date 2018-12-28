namespace LogDashboard.Route
{
    public class LogDashboardRoute
    {
        public LogDashboardRoute(string key, string view)
        {
            HtmlView = true;
            Key = key;
            View = view;

        }

        public LogDashboardRoute()
        {

        }

        public string Key { get; set; }

        public string View { get; set; }

        public string Handle { get; set; }

        public string Action { get; set; }

        public bool HtmlView { get; set; }

    }
}
