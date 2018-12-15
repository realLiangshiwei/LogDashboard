using NLogDashboard.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NLogDashboard.Repository
{
    public class FileRepository<T> : IRepository<T> where T : class, ILogModel
    {
        private readonly List<T> _data;

        public FileRepository()
        {

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
