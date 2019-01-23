using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogDashboard.Extensions;
using LogDashboard.Models;

namespace LogDashboard.Repository.File
{
    public class FileUnitOfWork<T> : IUnitOfWork where T : ILogModel, new()
    {
        private List<T> _logs;

        private readonly LogDashboardOptions _options;

        /// <summary>
        /// 日志模板完整标识
        /// </summary>
        public bool LogModelCompletion = true;

        public FileUnitOfWork(LogDashboardOptions options)
        {
            _options = options;
            _logs = new List<T>();
        }

        public List<T> GetLogs()
        {
            return _logs;
        }

        public async Task Open()
        {
            await ReadLogs();
        }

        public void Close()
        {
            _logs = null;
        }

        private async Task ReadLogs()
        {
            int id = 1;
            var rootPath = _options.RootPath ?? AppContext.BaseDirectory;

            if (!Directory.Exists(rootPath))
            {
                _logs.Add(CreateWarnItem(id, $"{LogDashboardConsts.Root} Warn:日志文件目录不存在,请检查 LogDashboardOption.RootPath 配置!"));
                return;
            }
            var paths = Directory.GetFiles(rootPath, "*.log", SearchOption.AllDirectories);

            var logFiles = paths.Select(x => new { Path = x, LastWriteTime = System.IO.File.GetLastWriteTime(x) }).OrderBy(x => x.LastWriteTime).ToList();

            foreach (var logFile in logFiles)
            {
                var stringBuilder = new StringBuilder();

                using (var fileStream = new FileStream(logFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(await streamReader.ReadLineAsync());
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
                            Level = line.TryGetValue(1)?.ToUpper(),
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
