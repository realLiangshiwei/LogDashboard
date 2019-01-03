using LogDashboard.Model;

namespace DatabaseSource
{
    public class CustomLogModel : LogModel
    {
        public string MachineName { get; set; }

        public string Callsite { get; set; }
    }
}
