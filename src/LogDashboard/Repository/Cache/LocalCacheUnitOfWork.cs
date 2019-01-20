using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using LogDashboard.Models;

namespace LogDashboard.Repository.Cache
{
    public class LocalCacheUnitOfWork : ICacheUnitOfWork
    {
        public ConcurrentBag<string> PathList { get { return LogMemoryCache.PathList; } set { } }
        public ConcurrentBag<LogCacheInfo> FileList { get { return LogMemoryCache.FileList; } set { } }
        public ConcurrentDictionary<string, LogCacheInfo> CurrentLogInfos { get { return LogMemoryCache.CurrentLogInfos; } set { } }
        public ConcurrentDictionary<string, ILogModel> LogInfoCaches { get { return LogMemoryCache.LogInfoCaches; } set { } }

        public void ClearAllCache()
        {
            PathList = null;
            FileList = null;
            CurrentLogInfos = null;
            LogInfoCaches = null;
        }

        public void Dispose()
        {
            LogMemoryCache.PathList = new ConcurrentBag<string>();
            LogMemoryCache.FileList = new ConcurrentBag<LogCacheInfo>();
            LogMemoryCache.CurrentLogInfos = new ConcurrentDictionary<string, LogCacheInfo>();
            LogMemoryCache.LogInfoCaches = new ConcurrentDictionary<string, ILogModel>();
        }

        public List<ILogModel> GetAllCacheLogs()
        {
            var list = new List<ILogModel>();
            if (LogInfoCaches.Count >= 1)
            {
                foreach (var key in LogInfoCaches.Keys)
                {
                    list.Add(LogInfoCaches[key]);
                }
            }
            return list;
        }



        public List<ILogModel> GetPageLogsFromCache(SearchLogInput input)
        {
            throw new NotImplementedException();
        }

        public string[] GetPageLogsPaths(SearchLogInput input)
        {
            throw new NotImplementedException();
        }

        public string[] GetPathsByFileList(int startCount, int endCount)
        {
            throw new NotImplementedException();
        }

        public ConcurrentDictionary<string, ILogModel> WriteCacheByPolicy(CachePolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
