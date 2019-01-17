using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogDashboard.Models;

namespace RequestTracking
{
    public class RequestTraceLogModel : LogModel, IRequestTrackLogModel
    {
        public string TraceIdentifier { get; set; }
    }
}
