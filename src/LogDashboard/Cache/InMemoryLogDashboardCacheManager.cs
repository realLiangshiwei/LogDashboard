using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LogDashboard.Models;

namespace LogDashboard.Cache
{
    public class InMemoryLogDashboardCacheManager<T> : ILogDashboardCacheManager<T> where T : class, ILogModel
    {
        private readonly ConcurrentDictionary<string, List<T>> _caches;

        private Timer _timer;

        private readonly LogDashboardOptions _options;

        public InMemoryLogDashboardCacheManager(LogDashboardOptions options)
        {
            _options = options;
            this._caches = new ConcurrentDictionary<string, List<T>>();
        }

        public Task SetCache(string key, List<T> logs)
        {
            _timer ??= new Timer(async (e) => await ClearCache(LogDashboardConsts.LogDashboardLogsCache), null,
                _options.CacheExpires,
                _options.CacheExpires);
            _caches.AddOrUpdate(key, logs, (k, v) => logs);
            return Task.CompletedTask;

        }

        public Task ClearCache(string key)
        {
            _caches.TryRemove(key, out List<T> val);
            _timer?.Dispose();
            _timer = null;
            return Task.CompletedTask;
        }


        public Task<List<T>> GetCache(string key)
        {
            return Task.FromResult(_caches.GetOrAdd(key, add => new List<T>()));
        }
    }
}
