using NLogDashboard.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DapperExtensions;
using NLogDashboard.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

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
            var paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.log", SearchOption.AllDirectories);
            int id = 0;
            foreach (var path in paths)
            {
                var text = File.ReadAllText(path, Encoding.UTF8);
                var logLines = text.Trim().Split(new[] { "|end" }, StringSplitOptions.None);

                foreach (var logLine in logLines)
                {
                    var line = logLine.Split('|');
                    if (line.Length > 1)
                    {
                        T item = new T
                        {
                            Id = id,
                            LongDate = DateTime.Parse(line[0]),
                            Level = line.TryGetValue(1).ToUpper(),
                            Logger = line.TryGetValue(2),
                            Message = line.TryGetValue(3),
                            Exception = line.TryGetValue(4).Trim()
                        };

                        var typeProperties = item.GetType().GetProperties();

                        for (var i = 5; i < line.Length; i++)
                        {
                            typeProperties[i].SetValue(item, line.TryGetValue(i));
                        }
                        _data.Add(item);
                        id++;
                    }

                }
            }

        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            return GetList(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return _data.Where(CheckPredicate(predicate).Compile()).ToList();

        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            return _data.Count(CheckPredicate(predicate).Compile());
        }

        private Expression<Func<T, bool>> CheckPredicate(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return x => x.Id != 0;
            }

            return predicate;
        }


        public IEnumerable<T> GetPageList(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts)
        {
            var query = _data.Where(CheckPredicate(predicate).Compile()).AsQueryable();
            foreach (var sort in sorts.Select((value, i) => new { i, value }))
            {
                var order = sort.value.Ascending ? "asc" : "desc";

                query = sort.i == 0 ? query.OrderBy($"{sort.value.PropertyName} {order}") : ((IOrderedQueryable<T>)query).ThenBy($"{sort.value.PropertyName} {order}");
            }
            return query.Skip(page - 1 * size).Take(size).ToList();
        }


    }
}
