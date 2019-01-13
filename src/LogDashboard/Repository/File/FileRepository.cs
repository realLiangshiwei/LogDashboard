using LogDashboard.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DapperExtensions;
using LogDashboard.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace LogDashboard.Repository.File
{
    public class FileRepository<T> : IRepository<T> where T : class, ILogModel, new()
    {

        private readonly List<T> _logs;
        public FileRepository(IUnitOfWork unitOfWork)
        {
            _logs = (unitOfWork as FileUnitOfWork<T>)?.GetLogs();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate = null)
        {
            return GetList(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return _logs.Where(CheckPredicate(predicate).Compile()).ToList();

        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            return _logs.Count(CheckPredicate(predicate).Compile());
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
            var query = _logs.Where(CheckPredicate(predicate).Compile()).AsQueryable();
            foreach (var sort in sorts.Select((value, i) => new { i, value }))
            {
                var order = sort.value.Ascending ? "asc" : "desc";

                query = sort.i == 0 ? query.OrderBy($"{sort.value.PropertyName} {order}") : ((IOrderedQueryable<T>)query).ThenBy($"{sort.value.PropertyName} {order}");
            }
            return query.Skip((page - 1) * size).Take(size).ToList();
        }


    }
}
