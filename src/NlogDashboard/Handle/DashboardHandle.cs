using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.StackTrace.Sources;
using NLogDashboard.Model;
using NLogDashboard.Repository;

namespace NLogDashboard.Handle
{
    public class DashboardHandle : NlogNLogDashboardHandleBase
    {
        private readonly ExceptionDetailsProvider _exceptionDetailsProvider;

        private readonly IRepository<ILogModel> _logRepository;

        public DashboardHandle(
            IServiceProvider serviceProvider,
            IRepository<ILogModel> logRepository) : base(serviceProvider)
        {
            _logRepository = logRepository;
            _exceptionDetailsProvider = new ExceptionDetailsProvider(new PhysicalFileProvider(AppContext.BaseDirectory), 6);
        }

        public async Task<string> Home()
        {
            var result = _logRepository.GetPageList(x => true, 1, 10, new Sort { Ascending = false, PropertyName = "longDate" });

            ViewBag.unique = _logRepository.GetList(x => true).GroupBy(x => x.Message).Count(x => x.Count() == 1);
            ViewBag.allCount = _logRepository.Count(x => true);

            var now = DateTime.Now;

            ViewBag.todayCount = _logRepository.Count(x =>
                   x.LongDate.Date >= now.Date && x.LongDate <= now.Date.AddHours(23).AddMinutes(59));

            var hour = now.AddHours(-1);
            ViewBag.hourCount = _logRepository.Count(x => x.LongDate >= hour && x.LongDate <= now);

            return await View(result);
        }


        //public async Task<string> SearchLog(SearchlogInput input)
        //{
        //    var result = await Conn.QueryAsync(BuildSql(input));
        //    return await View(result, "Views.Dashboard.LogList.cshtml");
        //}

        public async Task<string> LogInfo(EnttiyDto input)
        {
            var log = _logRepository.FirstOrDefault(x => x.Id == input.Id); ;
            return await View(log);
        }

        public  string GetException(EnttiyDto input)
        {
            var result = _logRepository.FirstOrDefault(x => x.Id == input.Id).Exception;
            return result;
        }

        public async Task<string> Ha()
        {
            try
            {
                throw new ArgumentException("测试一场", new Exception("关联异常"));
            }
            catch (Exception e)
            {
                var ex = _exceptionDetailsProvider.GetDetails(e);
                return await View(ex);
            }

        }

        public string BuildSql(SearchlogInput input)
        {
            if (input.All)
            {
                return $"select * from log order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.ToDay)
            {
                var now = DateTime.Now.ToShortDateString();
                return
                    $"select * from log where longdate>={now} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.Hour)
            {
                var now = DateTime.Now.AddHours(-1);

                return $"select * from log where longDate>={now} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.Unique)
            {
                return
                    $"select b.* from log b where b.Message in(select Message from log a group by a.Message having count(a.Message) = 1) order by b.Id desc  offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";

            }

            if (!string.IsNullOrWhiteSpace(input.Level))
            {
                return
                    $"select * from log where level= {input.Level} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }
            return $"select * from log order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
        }
    }
}
