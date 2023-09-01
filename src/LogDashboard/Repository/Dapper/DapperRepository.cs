using DapperExtensions;
using LogDashboard.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LogDashboard.Repository.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : class, ILogModel
    {
        private readonly DbConnection _conn;

        private readonly LogDashboardOptions _options;

        public DapperRepository(IUnitOfWork unitOfWork, LogDashboardOptions options)
        {
            _options = options;
            _conn = (unitOfWork as DapperUnitOfWork)?.GetConnection();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await _conn.FirstOrDefaultAsync<T>(predicate?.ToPredicateGroup());
        }


        public async Task<IEnumerable<T>> RequestTraceAsync(T model)
        {
            var traceIdentifier = ((IRequestTraceLogModel)model).TraceIdentifier;
            return await _conn.QueryAsync<T>(
                $"SELECT * FROM {_options.LogTableName} WHERE TraceIdentifier=@TraceIdentifier", new { traceIdentifier });

        }

        public async Task<(int Count, List<int> ids)> UniqueCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                var logs = (await _conn.GetListAsync<T>(predicate.ToPredicateGroup(), whereSql:
                     $"ID IN (SELECT MAX(id) FROM {_options.LogTableName} GROUP BY Message,Exception HAVING COUNT(*)=1)")).Select(x => x.Id).ToList();

                return (logs.Count, logs);
            }

            var result = await _conn.QueryAsync<int>(
                $"SELECT MAX(ID) AS TOTAL FROM {_options.LogTableName} GROUP BY Message,Exception HAVING COUNT(*)=1");
            return (result.Count(), result.ToList());

        }

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await _conn.GetListAsync<T>(predicate?.ToPredicateGroup());
        }


        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return await _conn.CountAsync<T>(predicate?.ToPredicateGroup());
        }

        public async Task<IEnumerable<TResult>> Query<TResult>(string sql, object param = null)
        {
            return await _conn.QueryAsync<TResult>(sql, param);
        }

        public async Task<IEnumerable<T>> GetPageListAsync(int page, int size, Expression<Func<T, bool>> predicate = null,
            Sort[] sorts = null, List<int> uniqueIds = null)
        {
            var appendSql = new StringBuilder();
            if (uniqueIds != null && uniqueIds.Count > 0)
            {
                appendSql.Append("ID IN (");
                appendSql.Append(string.Join(",", uniqueIds));

                appendSql.Append(")");
            }

            return await _conn.GetPageAsync<T>(predicate?.ToPredicateGroup(), sorts, page == 0 ? page : page - 1, size, appendSql.ToString().TrimEnd(','));

        }

        public async Task<IEnumerable<NewChartDataOutput>> GetLevelCount(ChartDataType chartDataType, DateTime beginTime, DateTime? endTime = null)
        {
            endTime = endTime ?? DateTime.Now;
            var dateLength = 0;
            var dateLast = "";
            switch (chartDataType)
            {
                case ChartDataType.Hour:
                    dateLength = 15;
                    dateLast = "0:00";
                    break;
                case ChartDataType.Day:
                    dateLength = 13;
                    dateLast = ":00:00";
                    break;
                case ChartDataType.Week:
                    dateLength = 10;
                    dateLast = " 00:00:00";
                    break;
                case ChartDataType.Month:
                    dateLength = 10;
                    dateLast = " 00:00:00";
                    break;
            }
            var sql = $"SELECT count(1) Count,Level,CONVERT(varchar({dateLength}),LongDate,120)+'{dateLast}' LongDate" +
                $" FROM {_options.LogTableName}" +
                $" where LongDate >= '{beginTime}' and LongDate <= '{endTime}'" +
                $" group by Level,CONVERT(varchar({dateLength}),LongDate,120)";
            var result = await _conn.QueryAsync<NewChartDataOutput>(sql);
            return result;
        }
    }
}
