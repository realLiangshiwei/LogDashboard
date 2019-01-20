using LogDashboard.Models;

namespace DatabaseSource
{
    public class CustomLogModel : LogModel, IRequestTraceLogModel
    {
        public string TraceIdentifier { get; set; }

        public string MachineName { get; set; }

        public string Callsite { get; set; }

    }
}
