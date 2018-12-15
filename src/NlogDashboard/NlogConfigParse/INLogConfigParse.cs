using System;
using System.Collections.Generic;
using System.Text;

namespace NLogDashboard.NlogConfigParse
{
    public interface INLogConfigParse
    {
        NlogConfigParseModel Parse(string nLogConfig);
    }
}
