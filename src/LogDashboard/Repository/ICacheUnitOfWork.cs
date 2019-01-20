using LogDashboard.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LogDashboard.Repository.Cache
{
    public interface ICacheUnitOfWork
    {
        /// <summary>
        /// 除了当天以外的所有加载的文件路径
        /// </summary>
        ConcurrentBag<string> PathList { get; set; }

        /// <summary>
        /// 缓存除了当天以为每一个文件，以及对应的总条数，唯一条数
        /// </summary>
        ConcurrentBag<LogCacheInfo> FileList { get; set; }

        /// <summary>
        /// 缓存当天的数据
        /// </summary>
        ConcurrentDictionary<string, LogCacheInfo> CurrentLogInfos { get; set; }

        /// <summary>
        /// 缓存(根据缓存策略不同，缓存不同数量的数据)
        /// </summary>
        ConcurrentDictionary<string, ILogModel> LogInfoCaches { get; set; }

        /// <summary>
        /// 根据查询条件获得对应的文件路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string[] GetPageLogsPaths(SearchLogInput input);

        /// <summary>
        /// 根据开始行号，和结束行号获取对应的文件名
        /// </summary>
        /// <param name="startCount"></param>
        /// <param name="endCount"></param>
        /// <returns></returns>
        string[] GetPathsByFileList(int startCount, int endCount);

        /// <summary>
        /// 读取所有的缓存
        /// </summary>
        /// <returns></returns>
        List<ILogModel> GetAllCacheLogs();

        /// <summary>
        /// 根据查询条件从缓存中读取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ILogModel> GetPageLogsFromCache(SearchLogInput input);

        /// <summary>
        /// 根据缓存策略写缓存
        /// </summary>
        ConcurrentDictionary<string, ILogModel> WriteCacheByPolicy(CachePolicy policy);

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void ClearAllCache();


        void Dispose(); 
    }
}
