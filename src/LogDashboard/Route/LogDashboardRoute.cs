using System;
using LogDashboard.Views;

namespace LogDashboard.Route
{
    public class LogDashboardRoute
    {
        public LogDashboardRoute(string key, Type view)
        {
            HtmlView = true;
            Key = key;
            View = view;

        }

        public LogDashboardRoute()
        {

        }

        public string Key { get; set; }

        public Type View { get; set; }

        public string Handle { get; set; }

        public string Action { get; set; }

        public bool HtmlView { get; set; }

    }
}
