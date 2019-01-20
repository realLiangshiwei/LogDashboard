using LogDashboard.Models;

namespace LogDashboard.Models
{
    public interface IRequestTraceLogModel : ILogModel
    {
        string TraceIdentifier { get; set; }
    }
}
