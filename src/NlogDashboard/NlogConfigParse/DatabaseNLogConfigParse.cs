using System;
using System.Collections.Generic;
using System.Text;

namespace NLogDashboard.NlogConfigParse
{
    public class DatabaseNLogConfigParse : INLogConfigParse
    {
        public List<string> Fields { get; set; }

        public NlogConfigParseModel Parse(string nLogConfig)
        {
            throw new NotImplementedException();
        }
    }
}
