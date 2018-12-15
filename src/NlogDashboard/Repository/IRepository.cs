using NLogDashboard.Model;
using System;
using System.Collections.Generic;

namespace NLogDashboard.Repository
{
    public interface IRepository<out T> where T : class, ILogModel
    {
        T FirstOrDefault(Func<T, bool> predicate);

        IEnumerable<T> GetList(Func<T, bool> predicate);

    }
}
