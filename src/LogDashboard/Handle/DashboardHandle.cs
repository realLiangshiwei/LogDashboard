using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DapperExtensions;
using LogDashboard.Extensions;
using LogDashboard.Handle.LogChart;
using LogDashboard.Models;
using LogDashboard.Repository;
using LogDashboard.Repository.Dapper;

namespace LogDashboard.Handle
{
    public class DashboardHandle<T> : LogDashboardHandleBase where T : class, ILogModel
    {
        private readonly IRepository<T> _logRepository;

        public DashboardHandle(
            IServiceProvider serviceProvider,
            IRepository<T> logRepository) : base(serviceProvider)
        {
            _logRepository = logRepository;
        }

        public async Task<string> Home()
        {
            ViewBag.dashboardNav = "active";
            ViewBag.basicLogNav = "";
            var result = await _logRepository.GetPageList(1, 10, sorts: new Sort { Ascending = false, PropertyName = "Id" });

            ViewBag.unique = (await _logRepository.GetList()).GroupBy(x => x.Message).Count(x => x.Count() == 1);
            var now = DateTime.Now;
            var weeHours = now.Date.AddHours(23).AddMinutes(59);
            ViewBag.todayCount = await _logRepository.Count(x => x.LongDate >= now.Date && x.LongDate <= weeHours);

            var hour = now.AddHours(-1);
            ViewBag.hourCount = await _logRepository.Count(x => x.LongDate >= hour && x.LongDate <= now);
            ViewBag.allCount = await _logRepository.Count();

            //Chart Data
            ViewBag.ChartData = (await LogChartFactory.GetLogChart(ChartDataType.Hour).GetCharts(_logRepository)).ToJsonString();

            return await View(result);
        }

        public async Task<string> GetLogChart(GetChartDataInput input)
        {
            return Json(await LogChartFactory.GetLogChart(input.ChartDataType).GetCharts(_logRepository));
        }


        public async Task<string> BasicLog(SearchLogInput input)
        {
            ViewBag.dashboardNav = "";
            ViewBag.basicLogNav = "active";
            if (input == null)
            {
                input = new SearchLogInput();
            }
            var result = await GetPageResult(input);
            ViewBag.logs = await View(result.List, "Views.Dashboard.LogList.cshtml");
            ViewBag.page = Html.Page(input.Page, input.PageSize, result.TotalCount);
            return await View();
        }

        public async Task<string> SearchLog(SearchLogInput input)
        {
            var result = await GetPageResult(input);
            ViewBag.totalCount = result.TotalCount;
            return Json(new SearchLogModel
            {
                Page = Html.Page(input.Page, input.PageSize, result.TotalCount),
                Html = await View(result.List, "Views.Dashboard.LogList.cshtml")
            });
        }

        private async Task<PagedResultModel<T>> GetPageResult(SearchLogInput input)
        {
            Expression<Func<T, bool>> expression = x => x.Id != 0;

            expression = expression.AndIf(input.ToDay, () =>
             {
                 var now = DateTime.Now;
                 var weeHours = now.Date.AddHours(23).AddMinutes(59);
                 return x => x.LongDate >= now.Date && x.LongDate <= weeHours;
             });

            expression = expression.AndIf(input.Hour, () =>
             {
                 var now = DateTime.Now;
                 var hour = now.AddHours(-1);
                 return x => x.LongDate >= hour && x.LongDate <= now;
             });

            expression = expression.AndIf(input.StartTime != null, () => { return x => x.LongDate >= input.StartTime.Value; });

            expression = expression.AndIf(input.EndTime != null, () => { return x => x.LongDate <= input.EndTime.Value; });

            expression = expression.AndIf(!string.IsNullOrWhiteSpace(input.Level), () => { return x => x.Level == input.Level; });

            expression = expression.AndIf(!string.IsNullOrWhiteSpace(input.Message), () => { return x => x.Message.Contains(input.Message); });

            if (input.Unique)
            {
                var query = await _logRepository.GetList(expression);
                return new PagedResultModel<T>(query.GroupBy(x => x.Message).Count(x => x.Count() == 1),
                    query.GroupBy(x => x.Message).Where(x => x.Count() == 1)
                        .SelectMany(x => x.ToList()).Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToList());
            }

            var logs = await _logRepository.GetPageList(input.Page, input.PageSize, expression, new Sort { Ascending = false, PropertyName = "Id" });

            var totalCount = await _logRepository.Count(expression);


            return new PagedResultModel<T>(totalCount, logs);
        }

        public async Task<string> LogInfo(T info)
        {
            return await View(info);
        }

        public async Task<string> RequestTrace(LogModelInput input)
        {
            var log = await _logRepository.FirstOrDefault(x => x.Id == input.Id);

            var traceIdentifier = ((IRequestTrackLogModel)log).TraceIdentifier;

            if (string.IsNullOrWhiteSpace(traceIdentifier))
            {
                return await View(new List<T>(), "Views.Dashboard.TraceLogList.cshtml");
            }

            if (Context.Options.DatabaseSource)
            {
                return await View(await ((DapperRepository<T>)_logRepository).Query(
                    $"select * from {Context.Options.LogTableName} where TraceIdentifier=@TraceIdentifier", new { traceIdentifier }), "Views.Dashboard.TraceLogList.cshtml");
            }

            return await View((await _logRepository
                .GetList(x =>
                    ((IRequestTrackLogModel)x).TraceIdentifier == traceIdentifier))
                .OrderBy(x => x.LongDate).ToList(), "Views.Dashboard.TraceLogList.cshtml");
        }
    }
}
