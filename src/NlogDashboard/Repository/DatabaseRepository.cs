using DapperExtensions;
using NLogDashboard.Model;
using NLogDashboard.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Dapper;

namespace NLogDashboard.Repository
{
    public class DatabaseRepository<T> : IRepository<T> where T : class, ILogModel
    {
        private readonly SqlConnection _conn;

        public DatabaseRepository(SqlConnection conn)
        {
            _conn = conn;
            _conn.Open();
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return GetList(predicate).FirstOrDefault();
        }

        public IEnumerable<T> GetList(Func<T, bool> predicate)
        {
            Expression<Func<T, bool>> exp = t => predicate(t);
            return _conn.GetList<T>(exp.ToPredicateGroup());

        }

        public int Count(Func<T, bool> predicate)
        {
            Expression<Func<T, bool>> exp = t => predicate(t);
            return _conn.Count<T>(exp);
        }

        public IEnumerable<T> GetPageList(Func<T, bool> predicate, int page, int size, params ISort[] sorts)
        {
            Expression<Func<T, bool>> exp = t => predicate(t);
            return _conn.GetPage<T>(exp, sorts, page, size);

        }
    }
}
