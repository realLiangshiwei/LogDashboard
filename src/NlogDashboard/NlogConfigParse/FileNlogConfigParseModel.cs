using System;
using System.Collections.Generic;
using System.Text;

namespace NLogDashboard.NlogConfigParse
{
    public class FileNlogConfigParseModel : NlogConfigParseModel
    {
        public string LayoutDelimiter { get; set; }

        public string FileName { get; set; }
    }
}
