using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NlogDashboard.Handler
{
    public interface INlogDashboardHandle
    {
        NlogDashboardContext Context { get; }
    }
}
