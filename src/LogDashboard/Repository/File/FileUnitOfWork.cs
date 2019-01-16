using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LogDashboard.Extensions;
using LogDashboard.Model;

namespace LogDashboard.Repository.File
{
    public class FileUnitOfWork<T> : IUnitOfWork where T : ILogModel, new()
    {
        private List<T> _logs;
        private readonly LogDashboardOptions _options;
        /// <summary>
        /// 日志模板不完整标识
        /// </summary>
        private bool LogModelNotCompletion = false;

        public FileUnitOfWork(LogDashboardOptions options)
        {
            _options = options;
            _logs = new List<T>();
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

        private void ReadLogs()
        {
            int id = 1;
            var rootPath = _options.RootPath ?? AppContext.BaseDirectory; 
            if (!Directory.Exists(rootPath))
            {
                _logs.Add(CreateWarnItem(id, "LogDashborad Warn:找不到日志目录，请检查配置。"));
                return;
            }
            var paths = Directory.GetFiles(rootPath, "*.log", SearchOption.AllDirectories); 

            foreach (var path in paths)
            {
                var stringBuilder = new StringBuilder();

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    while (!streamReader.EndOfStream)
                    {
                        stringBuilder.AppendLine(streamReader.ReadLine());
                    }
                }

                var text = stringBuilder.ToString();
                var logLines = text.Trim().Split(new[] { _options.FileEndDelimiter }, StringSplitOptions.None);

                foreach (var logLine in logLines)
                {
                    var line = logLine.Split(new[] { _options.FileFieldDelimiter }, StringSplitOptions.None);
                    if (line.Length > 1)
                    {
                        T item = new T
                        {
                            Id = id,
                            LongDate = DateTime.Parse(line.TryGetValue(0).ToUpper()),
                            Level = line.TryGetValue(1).ToUpper(),
                            Logger = line.TryGetValue(2),
                            Message = line.TryGetValue(3),
                            Exception = line.TryGetValue(4).Trim()
                        };
                        //避免日志项不只6个，但是未配置自定义字段造成索引超出界限的异常,并标记日志模型配置不完整
                         for (var i = 5; i < line.Length; i++)
                         {
                            if (i - 5 >= _options.CustomPropertyInfos.Count)
                            {
                                LogModelNotCompletion = true;
                                break;
                            }
                             _options.CustomPropertyInfos[i - 5].SetValue(item, line.TryGetValue(i));
                         }
                        
                        _logs.Add(item);
                        id++;
                    }

                }
            }
            
            if (LogModelNotCompletion)
            {
                _logs.Add(CreateWarnItem(id, "LogDashborad Warn:自定义日志模型配置不完整。"));
                LogModelNotCompletion = false;
            }

        }
        
        public T CreateWarnItem(int id, string message)
        {
            return new T
            {
                Id = id,
                Logger="LogDashboard",
                LongDate = DateTime.Now,
                Level="WARN",
                Message= message
            };
        }
        
        public void Dispose()
        {
            Close();
        }
    }
}
