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

        protected static readonly List<LogFile> LogFiles = new List<LogFile>();

        public FileUnitOfWork(
            LogDashboardOptions options,
            ILogDashboardCacheManager<T> cacheManager)
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
                await ReadAllLogs();
            }
        }

        public void Close()
        {
            _logs = null;
        }

        private async Task ReadIncrementalLogs()
        {
            BuildLogFiles();
            var id = _logs.Max(x => x.Id);
            await ReadLogs(++id);
        }

        private void BuildLogFiles()
        {
            var rootPath = _options.RootPath ?? AppContext.BaseDirectory;

            if (!Directory.Exists(rootPath))
            {
                _logs.Add(CreateWarnItem(_logs.Last().Id + 1, $"{LogDashboardConsts.Root} Warn:日志文件目录不存在,请检查 LogDashboardOption.RootPath 配置!"));
            }

            var paths = Directory.GetFiles(rootPath, "*.log", SearchOption.AllDirectories);

            var logFiles = paths.Select(x => new LogFile
            {
                Path = x,
                LastModifyTime = System.IO.File.GetLastWriteTime(x),
                LastReadLine = 0
            }).ToList();

            if (!LogFiles.Any())
            {
                LogFiles.AddRange(logFiles);
            }
            else
            {
                foreach (var logFile in logFiles)
                {
                    var temp = LogFiles.FirstOrDefault(x => x.Path == logFile.Path);
                    if (temp == null)
                    {
                        LogFiles.AddRange(logFiles);
                        continue;
                    }

                    if (temp.LastModifyTime != logFile.LastModifyTime)
                    {
                        temp.ShouldRead = true;
                    }
                }
            }
        }

        private async Task ReadLogs(int id = 1)
        {

            foreach (var logFile in LogFiles.Where(x=>x.ShouldRead))
            {
                var stringBuilder = new StringBuilder();

				//安装包System.Text.Encoding.CodePages
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var fileStream = new FileStream(logFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.GetEncoding(0)))
                {
                    //Skip line
                    for (var i = 0; i < logFile.LastReadLine; i++)
                    {
                        await streamReader.ReadLineAsync();
                    }

                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(await streamReader.ReadLineAsync());
                        logFile.LastReadLine++;
                    }
                }

                var text = stringBuilder.ToString();
                var logLines = text.Trim().Replace("|| end", _options.FileEndDelimiter)
                    .Split(new[] {_options.FileEndDelimiter}, StringSplitOptions.None)
                    .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                foreach (var logLine in logLines)
                {
                    var line = logLine.Split(new[] {_options.FileFieldDelimiter}, StringSplitOptions.None);
                    if (line.Length > 1)
                    {
                        var item = new T
                        {
                            Id = id,
                            LongDate = DateTime.Parse(line.TryGetValue(0)),
                            Logger = line.TryGetValue(2),
                            Message = line.TryGetValue(3),
                            Exception = line.TryGetValue(4),
                            Level = line.TryGetValue(1)?.ToUpper().FormatLevel()
                        };

                        var lineEnd = Math.Min(_options.CustomPropertyInfos.Count, line.Length - 5);
                        if (line.Length - 5 != _options.CustomPropertyInfos.Count && logLine == logLines.Last())
                        {
                            _logs.Add(CreateWarnItem(id, $"Warn: {Path.GetFileName(logFile.Path)} 文件内容与自定义日志模型不完全匹配,请检查代码!"));
                        }

                        for (var i = 0; i < lineEnd; i++)
                        {
                            _options.CustomPropertyInfos[i].SetValue(item, line.TryGetValue(i + 5));
                        }

                        _logs.Add(item);
                        id++;
                    }
                }

                logFile.ShouldRead = false;
            }

            await _cacheManager.SetCache(LogDashboardConsts.LogDashboardLogsCache, _logs);
        }

        private async Task ReadAllLogs()
        {
            BuildLogFiles();
            await ReadLogs();
        }

        private T CreateWarnItem(int id, string message)
        {
            return new T
            {
                Id = id,
                Logger = LogDashboardConsts.Root,
                LongDate = DateTime.Now,
                Level = LogLevelConst.Warn,
                Message = message
            };
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class LogFile
    {
        public string Path { get; set; }

        public int LastReadLine { get; set; }

        public DateTime LastModifyTime { get; set; }

        public bool ShouldRead { get; set; } = true;
    }
}
