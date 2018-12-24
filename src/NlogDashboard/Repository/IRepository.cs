using NLogDashboard.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperExtensions;

namespace NLogDashboard.Repository
{
    public interface IRepository<T> where T : class, ILogModel
    {
        T FirstOrDefault(Expression<Func<T, bool>> predicate = null);

        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null);

        int Count(Expression<Func<T, bool>> predicate = null);

        IEnumerable<T> GetPageList(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts);

    }

}
