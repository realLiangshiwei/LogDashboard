using LogDashboard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DapperExtensions;
using LogDashboard.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogDashboard.Repository.File
{
    public class FileRepository<T> : IRepository<T> where T : class, ILogModel, new()
    {

        private readonly List<T> _logs;
        public FileRepository(IUnitOfWork unitOfWork)
        {
            _logs = (unitOfWork as FileUnitOfWork<T>)?.GetLogs();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.FromResult((await GetListAsync(predicate)).FirstOrDefault());
        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.FromResult(_logs.Where(CheckPredicate(predicate).Compile()).ToList());

        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await Task.FromResult(_logs.Count(CheckPredicate(predicate).Compile()));
        }

        private Expression<Func<T, bool>> CheckPredicate(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return x => x.Id != 0;
            }

            return predicate;
        }


        public async Task<IEnumerable<T>> GetPageListAsync(int page, int size, Expression<Func<T, bool>> predicate = null, params ISort[] sorts)
        {
            var query = _logs.Where(CheckPredicate(predicate).Compile()).AsQueryable();
            foreach (var sort in sorts.Select((value, i) => new { i, value }))
            {
                var order = sort.value.Ascending ? "asc" : "desc";

                query = sort.i == 0 ? query.OrderBy($"{sort.value.PropertyName} {order}") : ((IOrderedQueryable<T>)query).ThenBy($"{sort.value.PropertyName} {order}");
            }
            return await Task.FromResult(query.Skip((page - 1) * size).Take(size).ToList());
        }


    }
}
