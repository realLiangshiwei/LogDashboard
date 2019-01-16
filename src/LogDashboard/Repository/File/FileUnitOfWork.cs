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
            var paths = Directory.GetFiles(_options.RootPath??AppContext.BaseDirectory, "*.log", SearchOption.AllDirectories);
            int id = 1;

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


                         //避免日志项不只6个，但是未配置自定义字段造成索引超出界限的异常

                        if (_options.CustomPropertyInfos.Count > 0)
                        {
                            for (var i = 5; i < line.Length; i++)
                            {
    
                                _options.CustomPropertyInfos[i - 5].SetValue(item, line.TryGetValue(i));
                            }
                        }
                        _logs.Add(item);
                        id++;
                    }

                }
            }

        }

        public void Dispose()
        {
            Close();
        }
    }
}
