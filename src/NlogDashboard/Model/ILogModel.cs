using System;

namespace NLogDashboard.Model
{
    public interface ILogModel
    {
        DateTime LongDate { get; set; }

        string Level { get; set; }

        string Message { get; set; }

        string Logger { get; set; }

        string Exception { get; set; }
    }
}
