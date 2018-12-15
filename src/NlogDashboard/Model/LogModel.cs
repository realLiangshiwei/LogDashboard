using System;

namespace NLogDashboard.Model
{
    public class LogModel : ILogModel
    {
        public DateTime LongDate { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }
    }
}
