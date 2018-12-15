using System;
using System.Collections.Generic;
using System.Text;

namespace NLogDashboard.NlogConfigParse
{
    public class NlogConfigParseModel
    {
        public List<string> Fields { get; set; }

        public NlogConfigParseModel()
        {
            Fields = new List<string>();
        }
    }
}
