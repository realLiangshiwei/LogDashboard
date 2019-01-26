using System.Collections.Generic;
using System.Threading.Tasks;
using LogDashboard.Models;

namespace LogDashboard.Cache
{
    public interface ILogDashboardCacheManager<T> where T : class, ILogModel
    {
        Task SetCache(string key, List<T> logs);

        Task ClearCache(string key);

        Task<List<T>> GetCache(string key);
    }
}
