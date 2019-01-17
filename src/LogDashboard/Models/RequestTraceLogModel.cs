namespace LogDashboard.Models
{
    public class RequestTraceLogModel : LogModel, IRequestTrackLogModel
    {
        public string TraceIdentifier { get; set; }
    }
}
