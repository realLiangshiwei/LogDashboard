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
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);

        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        Task<IEnumerable<T>> GetPageListAsync(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts);

    }

}
