using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogDashboard.Extensions;
using LogDashboard.Models;
using LogDashboard.Repository.Cache;

namespace LogDashboard.Repository.File
{
    public class FileUnitOfWork<T> : IUnitOfWork where T : ILogModel, new()
    {
        private List<T> _logs;

        private readonly LogDashboardOptions _options;
 
        private Dictionary<string, string> itemInfo = new Dictionary<string, string>();

        #region 从缓存接口拿到的对象,默认
        private ConcurrentBag<string> PathList { get;  set; }
        private ConcurrentBag<LogCacheInfo> FileList { get;  set; }
        private ConcurrentDictionary<string, LogCacheInfo> CurrentLogInfos { get;  set; }
        private ConcurrentDictionary<string, ILogModel> LogInfoCaches { get;  set; }
        public ICacheUnitOfWork CacheUnitOfWork { get; private set; }
        #endregion

        /// <summary>
        /// 日志模板完整标识
        /// </summary>
        public bool LogModelCompletion = true;

        public FileUnitOfWork(LogDashboardOptions options)
        {
            _options = options;
            _logs = new List<T>();
            Open();
        }

        public FileUnitOfWork(LogDashboardOptions options, ICacheUnitOfWork cacheUnitOfWork)
        {
            _options = options;
            _logs = new List<T>();
            PathList = cacheUnitOfWork.PathList;
            FileList = cacheUnitOfWork.FileList;
            CurrentLogInfos = cacheUnitOfWork.CurrentLogInfos;
            LogInfoCaches= cacheUnitOfWork.LogInfoCaches;
            CacheUnitOfWork = cacheUnitOfWork;
            Open();
        }

        public List<T> GetLogs()
        {

            return _logs;
        }

        public void Open()
        {
            ReadLogs();
        }

        public void Close()
        {
            _logs = null;
        }

        #region 读取增量的Log
        private void ReadLogs()
        {
            int id = 1;
            var rootPath = _options.RootPath ?? AppContext.BaseDirectory;

            if (!Directory.Exists(rootPath))
            {
                _logs.Add(CreateWarnItem(id, $"{LogDashboardConsts.Root} Warn:日志文件目录不存在,请检查 LogDashboardOption.RootPath 配置!"));
                return;
            }
            string[] paths = Directory.GetFiles(rootPath, "*.log", SearchOption.AllDirectories);

            //如果缓存的文件路径列表不为空，则读取除文件列表以为的文件
            //if (LogMemoryCache.PathList.Count > 0 && LogMemoryCache.PathList.Count == LogMemoryCache.FileList.Count)
            if (PathList != null&& FileList!=null&&PathList.Count() > 0 && PathList.Count() == FileList.Count())
            {
                //paths = paths.Except(LogMemoryCache.PathList).ToArray();
                paths = paths.Except(PathList).ToArray();
            }

            ReadLogByPath(paths);
            if (!LogModelCompletion)
            {
                _logs.Add(CreateWarnItem(id, $"{LogDashboardConsts.Root} Warn:自定义日志模型与Config不完全匹配,请检查代码!"));
            }

        }
        #endregion 

        /// <summary>
        /// 根据路径读取文件
        /// </summary>
        /// <param name="paths"></param>
        public void ReadLogByPath(string[] paths)
        {
            int id = 1;
            var logFiles = paths.Select(x => new { Path = x, LastWriteTime = System.IO.File.GetLastWriteTime(x) }).OrderBy(x => x.LastWriteTime).ToList();
            foreach (var logFile in logFiles)
            {
                var stringBuilder = new StringBuilder();

                using (var fileStream = new FileStream(logFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(streamReader.ReadLine());
                    }
                }

                var text = stringBuilder.ToString();
                var logLines = text.Trim().Split(new[] { _options.FileEndDelimiter }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                int count = 0;
                int disCount = 0;
                foreach (var logLine in logLines)
                {
                    var line = logLine.Split(new[] { _options.FileFieldDelimiter }, StringSplitOptions.None);


                    if (line.Length > 1)
                    {
                        var item = new T
                        {
                            Id = id,
                            LongDate = Convert.ToDateTime(line.TryGetValue(0)),  //Convert.ToDateTime(line.TryGetValue(0)),
                            Level = line.TryGetValue(1)?.ToUpper(),
                            Logger = line.TryGetValue(2),
                            Message = line.TryGetValue(3),
                            Exception = line.TryGetValue(4)
                        };

                        //Message不一样则视作唯一
                        if (!itemInfo.ContainsKey(line.TryGetValue(3)?.GetMD5()))
                        {
                            itemInfo.Add(line.TryGetValue(3)?.GetMD5(), "");
                            disCount++;
                        }

                        var lineEnd = Math.Min(_options.CustomPropertyInfos.Count, line.Length - 5);
                        if (line.Length - 5 != _options.CustomPropertyInfos.Count && logFile == logFiles.Last() && logLine == logLines.Last())
                        {
                            //last files and last line
                            LogModelCompletion = false;
                        }

                        for (var i = 0; i < lineEnd; i++)
                        {
                            _options.CustomPropertyInfos[i].SetValue(item, line.TryGetValue(i + 5));
                        }

                        _logs.Add(item);
                        id++;
                        count++;
                    }
                }

                if (count < 1)
                    return;
                CurrentLogInfos.Clear();
                //如果不是今天的文件并且不包含则加入到缓存历史中
                //if (!FilePatternToday(logFile.Path) && !LogMemoryCache.PathList.Contains(logFile.Path))
                if (!FilePatternToday(logFile.Path) && !PathList.Contains(logFile.Path))
                {

                    //LogMemoryCache.PathList.Add(logFile.Path);
                    //LogMemoryCache.FileList.Add( new LogCacheInfo
                    //{
                    //    Path = logFile.Path,
                    //    Count = count,
                    //    DistinctCount = disCount
                    //});
                    PathList.Add(logFile.Path);
                    FileList.Add(new LogCacheInfo
                   {
                       Path = logFile.Path,
                       Count = count,
                       DistinctCount = disCount
                   });
                }
                else
                {
                    //如果是今天的，则加入到今天的增量中

                    //LogMemoryCache.CurrentLogInfos.AddOrUpdate(logFile.Path, 
                    //                                new LogCacheInfo{Path = logFile.Path,
                    //                                            Count = count,
                    //                                            DistinctCount = disCount}, 
                    //                                (x, y) =>{
                    //                                          if (LogMemoryCache.CurrentLogInfos.ContainsKey(x))
                    //                                            y.MD5 = text.GetMD5();
                    //                                            return y;});
                    CurrentLogInfos.TryAdd(logFile.Path, new LogCacheInfo
                    {
                        Path = logFile.Path,
                        Count = count,
                        DistinctCount = disCount
                    });
                }

            }
        }

        /// <summary>
        /// 根据文件名判断文件是否是今天的
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FilePatternToday(string path)
        {
   
            return false;
        }

        public T CreateWarnItem(int id, string message)
        {
            return new T
            {
                Id = id,
                Logger = LogDashboardConsts.Root,
                LongDate = DateTime.Now,
                Level = "WARN",
                Message = message
            };
        }

        public void Dispose()
        {
            Close();
        }
    }
}
