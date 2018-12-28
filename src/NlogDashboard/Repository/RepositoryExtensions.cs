using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogDashboard.Repository
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool exec,
            Expression<Func<T, bool>> expression)
        {
            return exec ? enumerable.Where(expression.Compile()) : enumerable;
        }
    }
}
