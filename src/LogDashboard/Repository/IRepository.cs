using LogDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DapperExtensions;

namespace LogDashboard.Repository
{
    public interface IRepository<T> where T : class, ILogModel
    {
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate = null);

        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate = null);

        Task<int> Count(Expression<Func<T, bool>> predicate = null);

        Task<IEnumerable<T>> GetPageList(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts);

    }

}
