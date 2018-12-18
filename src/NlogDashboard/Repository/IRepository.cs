using NLogDashboard.Model;
using System;
using System.Collections.Generic;
using DapperExtensions;

namespace NLogDashboard.Repository
{
    public interface IRepository<out T> where T : class, ILogModel
    {
        T FirstOrDefault(Func<T, bool> predicate);

        IEnumerable<T> GetList(Func<T, bool> predicate);

        int Count(Func<T, bool> predicate);

        IEnumerable<T> GetPageList(Func<T, bool> predicate, int page, int size, params ISort[] sorts);

    }
}
