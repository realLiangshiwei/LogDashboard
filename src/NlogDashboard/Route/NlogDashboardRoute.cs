namespace NlogDashboard.Route
{
    public class NlogDashboardRoute
    {
        public NlogDashboardRoute(string key, string view)
        {
            HtmlView = true;
            Key = key;
            View = view;

        }

        public NlogDashboardRoute()
        {

        }

        public string Key { get; set; }

        public string View { get; set; }

        public string Handle { get; set; }

        public string Action { get; set; }

        public bool HtmlView { get; set; }

    }
}
