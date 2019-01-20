namespace LogDashboard.Models
{
    public class RequestTraceLogModel : LogModel, IRequestTraceLogModel
    {
        public string TraceIdentifier { get; set; }
    }
}
