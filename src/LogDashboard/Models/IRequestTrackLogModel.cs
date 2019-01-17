using LogDashboard.Models;

namespace LogDashboard.Models
{
    public interface IRequestTrackLogModel : ILogModel
    {
        string TraceIdentifier { get; set; }
    }
}
