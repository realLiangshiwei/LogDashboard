using DapperExtensions;
using NLogDashboard.Model;
using System;
using System.Linq.Expressions;

namespace NLogDashboard.Repository.Dapper
{
    internal static class DapperExpressionExtensions
    {

        public static IPredicate ToPredicateGroup<T>(this Expression<Func<T, bool>> expression) where T : class, ILogModel
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var dev = new DapperExpressionVisitor<T>();

            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}
