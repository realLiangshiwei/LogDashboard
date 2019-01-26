using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogDashboard.Cache;
using LogDashboard.Extensions;
using LogDashboard.Models;

namespace LogDashboard.Repository.File
{
    public class FileUnitOfWork<T> : IUnitOfWork where T : class, ILogModel, new()
    {
        private List<T> _logs;

        private readonly LogDashboardOptions _options;

        private readonly ILogDashboardCacheManager<T> _cacheManager;


        private static int _lineNumber;

        private static DateTime? _lastFileWriteTime;

        private static string _lastFileName;

        /// <summary>
        /// 日志模板完整标识
        /// </summary>
        public bool LogModelCompletion = true;

        public FileUnitOfWork(LogDashboardOptions options, ILogDashboardCacheManager<T> cacheManager)
        {
            _options = options;
            _cacheManager = cacheManager;
            _logs = new List<T>();
        }

        public List<T> GetLogs()
        {
            return _logs;
        }

        public async Task Open()
        {
            _logs = await _cacheManager.GetCache(LogDashboardConsts.LogDashboardLogsCache);

            if (_logs.Count > 0)
            {
                await ReadIncrementalLogs();
            }
            else
            {
                _lineNumber = 0;
                _lastFileWriteTime = null;
                _lastFileName = null;
                await ReadAllLogs();
            }
        }

        public void Close()
        {
            _logs = null;
        }

        private async Task ReadIncrementalLogs()
        {
            var logFiles = GetLogFiles();

            logFiles.RemoveAll(x => x.LastWriteTime < _lastFileWriteTime);

            if (_lastFileName != logFiles.FirstOrDefault().Path)
            {
                _lineNumber = 0;
                logFiles.Remove(logFiles.FirstOrDefault());
            }

            await ReadLogs(logFiles, _logs.Last().Id++);
        }

        private List<(string Path, DateTime LastWriteTime)> GetLogFiles()
        {
            var rootPath = _options.RootPath ?? AppContext.BaseDirectory;

            if (!Directory.Exists(rootPath))
            {
                _logs.Add(CreateWarnItem(_logs.Last().Id + 1, $"{LogDashboardConsts.Root} Warn:日志文件目录不存在,请检查 LogDashboardOption.RootPath 配置!"));
                return new List<(string Path, DateTime LastWriteTime)>();
            }

            var paths = Directory.GetFiles(rootPath, "*.log", SearchOption.AllDirectories);

            return paths.Select(x => (Path: x, LastWriteTime: System.IO.File.GetLastWriteTime(x))).OrderBy(x => x.LastWriteTime).ToList();
        }

        private async Task ReadLogs(List<(string Path, DateTime LastWriteTime)> logFiles, int id = 1)
        {
            if (_lastFileWriteTime == logFiles.LastOrDefault().LastWriteTime)
            {
                return;
            }

            _lastFileWriteTime = logFiles.LastOrDefault().LastWriteTime;
            _lastFileName = logFiles.LastOrDefault().Path;

            foreach (var logFile in logFiles)
            {
                var stringBuilder = new StringBuilder();
                var fileLine = 0;
                using (var fileStream = new FileStream(logFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    //Skip line
                    for (var i = 0; i < _lineNumber; i++)
                    {
                        await streamReader.ReadLineAsync();
                    }


                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(await streamReader.ReadLineAsync());
                        fileLine++;
                    }
                }

                if (logFile == logFiles.Last())
                {
                    if (_lastFileName == logFile.Path)
                    {
                        _lineNumber += fileLine;
                    }
                    else
                    {
                        _lineNumber = fileLine;
                    }

                }

                var text = stringBuilder.ToString();
                var logLines = text.Trim().Split(new[] { _options.FileEndDelimiter }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                foreach (var logLine in logLines)
                {
                    var line = logLine.Split(new[] { _options.FileFieldDelimiter }, StringSplitOptions.None);
                    if (line.Length > 1)
                    {
                        var item = new T
                        {
                            Id = id,
                            LongDate = DateTime.Parse(line.TryGetValue(0)),
                            Level = line.TryGetValue(1)?.ToUpper()?.FormatLevel(),
                            Logger = line.TryGetValue(2),
                            Message = line.TryGetValue(3),
                            Exception = line.TryGetValue(4)
                        };

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
                    }

                }
            }

            if (!LogModelCompletion)
            {
                _logs.Add(CreateWarnItem(id, $"{LogDashboardConsts.Root} Warn:自定义日志模型与Config不完全匹配,请检查代码!"));
            }

            await _cacheManager.SetCache(LogDashboardConsts.LogDashboardLogsCache, _logs);
        }

        private async Task ReadAllLogs()
        {
            var logFiles = GetLogFiles();
            await ReadLogs(logFiles);
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
