using LogDashboard.Repository.Cache;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LogDashboard.Models
{
    /// <summary>
    /// 以增量的形式，通过缓存策略在内存中进行适量的缓存
    /// 也可以通过对加载历史的缓存，有选择的减少文件读取数量
    /// </summary>
    public static class LogMemoryCache
    {
        /// <summary>
        /// 缓存除了当天以外的所有加载的文件路径
        /// </summary>
        public static ConcurrentBag<string> PathList { get; set; } = new ConcurrentBag<string>();

        /// <summary>
        /// 缓存除了当天以为每一个文件，以及对应的总条数，唯一条数
        /// </summary>
        public static ConcurrentBag<LogCacheInfo> FileList { get; set; } = new ConcurrentBag<LogCacheInfo>();

        /// <summary>
        /// 缓存当天的文件信息
        /// </summary>
        public static ConcurrentDictionary<string,LogCacheInfo> CurrentLogInfos { get; set; } = new ConcurrentDictionary<string,LogCacheInfo>();

        /// <summary>
        /// 缓存(根据缓存策略不同，缓存少量的数据)
        /// 这里键用什么比较好。。。
        /// </summary>
        public static ConcurrentDictionary<string, ILogModel> LogInfoCaches { get; set; } = new ConcurrentDictionary<string,ILogModel>();
    }

}
