using System;
using System.Collections.Generic;
using System.Text;

namespace NLogDashboard.NlogConfigParse
{
    public interface INLogConfigParse
    {
        ILogConfigOptions Parse(string nLogConfig);
    }
}
