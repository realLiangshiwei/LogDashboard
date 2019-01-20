using System;
using System.Collections.Generic;
using System.Text;

namespace LogDashboard.Models
{
    /// <summary>
    /// 日志缓存信息
    /// </summary>
    public class LogCacheInfo
    {
        public string Path { get; set; }

        public string MD5 { get; set; }

        public long Count { get; set; }

        public long DistinctCount { get; set; }
    }
}
