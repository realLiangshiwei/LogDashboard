using NLogDashboard.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLogDashboard.Extensions;

namespace NLogDashboard.Repository
{
    public class FileRepository<T> : IRepository<T> where T : class, ILogModel, new()
    {
       
        private readonly List<T> _data;

        public FileRepository()
        {
            _data = new List<T>();
            ReadLogs();
        }

        private void ReadLogs()
        {
            var paths = Directory.GetFiles("Directory.GetCurrentDirectory()", "*.log", SearchOption.AllDirectories);

            foreach (var path in paths)
            {
                var text = File.ReadAllText(path, Encoding.UTF8);
                var logLines = text.Split(new[] { "|end" }, StringSplitOptions.None);

                foreach (var logLine in logLines)
                {
                    var properties = logLine.Split('|');
                    T item = new T
                    {
                        LongDate = DateTime.Parse(properties[0]),
                        Level = properties.TryGetValue(1),
                        Logger = properties.TryGetValue(2),
                        Message = properties.TryGetValue(3),
                        Exception = properties.TryGetValue(4)
                    };

                    var typeProperties = item.GetType().GetProperties();

                    for (var i = 4; i < properties.Length; i++)
                    {
                        typeProperties[i].SetValue(item, properties.TryGetValue(i));
                    }
                    _data.Add(item);
                }
            }

        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return GetList(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetList(Func<T, bool> predicate)
        {
            return _data.Where(predicate).ToList();

        }
    }
}
