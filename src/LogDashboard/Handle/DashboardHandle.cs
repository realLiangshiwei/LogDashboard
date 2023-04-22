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
using LogDashboard.Route;
using LogDashboard.Views.Dashboard;
using Microsoft.AspNetCore.Http;

namespace LogDashboard.Handle
{
    public class DashboardHandle<T> : LogDashboardHandleBase where T : class, ILogModel
    {
        private readonly IRepository<T> _logRepository;
        private readonly LogDashboardOptions _options;

        public DashboardHandle(
            IServiceProvider serviceProvider,
            IRepository<T> logRepository,
            LogDashboardOptions options) : base(serviceProvider)
        {
            _logRepository = logRepository;
            _options = options;
        }

        public async Task<string> Login(LoginInput input)
        {
            foreach (var filter in _options.AuthorizationFiles)
            {
                if (filter is LogdashboardAccountAuthorizeFilter accountFilter && accountFilter.Password == input?.Password && accountFilter.UserName == input?.Name)
                {
                    var timestamp = DateTime.Now.ToUnixTimestamp().ToString();
                    var token = $"{accountFilter.UserName}&&{accountFilter.Password}&&{timestamp}".ToMD5();
                    Context.HttpContext.Response.Cookies.Append(LogDashboardConsts.CookieTokenKey, token, new CookieOptions() { Expires = DateTime.Now.AddHours(accountFilter.LoginExpireHour) });
                    Context.HttpContext.Response.Cookies.Append(LogDashboardConsts.CookieTimestampKey, timestamp, new CookieOptions() { Expires = DateTime.Now.AddHours(accountFilter.LoginExpireHour) });
                    var homeUrl = LogDashboardRoutes.Routes.GetHomeRoute().Key;
                    Context.HttpContext.Response.Redirect($"{_options.PathMatch}{homeUrl}");
                    return string.Empty;
                }
            }

            return await View();

        }

        public async Task<string> Home()
        {
            ViewData["dashboardNav"] = "active";
            ViewData["basicLogNav"] = "";
            var result = await _logRepository.GetPageListAsync(1, 10, sorts: new[] { new Sort { Ascending = false, PropertyName = "Id" } });

            ViewData["unique"] = (await _logRepository.UniqueCountAsync()).Count;

            var now = DateTime.Now;
            var weeHours = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            ViewData["todayCount"] = await _logRepository.CountAsync(x => x.LongDate >= now.Date && x.LongDate <= weeHours);

            var hour = now.AddHours(-1);
            ViewData["hourCount"] = await _logRepository.CountAsync(x => x.LongDate >= hour && x.LongDate <= now);
            ViewData["allCount"] = await _logRepository.CountAsync();

            //Chart Data
            ViewData["ChartData"] = (await LogChartFactory.GetLogChart(ChartDataType.Hour).GetCharts(_logRepository)).ToJsonString();

            return await View(result);
        }

        public async Task<string> GetLogChart(GetChartDataInput input)
        {
            return Json(await LogChartFactory.GetLogChart(input.ChartDataType).GetCharts(_logRepository));
        }


        public async Task<string> BasicLog(SearchLogInput input)
        {
            ViewData["dashboardNav"] = "";
            ViewData["basicLogNav"] = "active";
            if (input == null)
            {
                input = new SearchLogInput();
            }
            var result = await GetPageResult(input);
            ViewData["logs"] = await View(result.List, typeof(LogList));
            ViewData["page"] = Html.Page(input.Page, input.PageSize, result.TotalCount);
            return await View();
        }

        public async Task<string> SearchLog(SearchLogInput input)
        {
            var result = await GetPageResult(input);
            ViewData["totalCount"] = result.TotalCount;
            return Json(new SearchLogModel
            {
                Page = Html.Page(input.Page, input.PageSize, result.TotalCount),
                Html = await View(result.List, typeof(LogList))
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

            expression = expression.AndIf(!string.IsNullOrWhiteSpace(input.Message), () =>
            {
                return x => x.Message.Contains(input.Message) || x.Logger.Contains(input.Message);
            });

            if (input.Unique)
            {
                var uniqueLogs = await _logRepository.UniqueCountAsync(expression);

                return new PagedResultModel<T>(uniqueLogs.Count, await _logRepository.GetPageListAsync(input.Page, input.PageSize, expression, new[] { new Sort { Ascending = false, PropertyName = "Id" } }, uniqueLogs.ids));
            }

            var logs = await _logRepository.GetPageListAsync(input.Page, input.PageSize, expression, new[] { new Sort { Ascending = false, PropertyName = "Id" } });

            var totalCount = await _logRepository.CountAsync(expression);


            return new PagedResultModel<T>(totalCount, logs);
        }

        public async Task<string> LogInfo(T info)
        {
            return await View(info);
        }

        public async Task<string> RequestTrace(LogModelInput input)
        {
            var log = await _logRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            var traceIdentifier = ((IRequestTraceLogModel)log).TraceIdentifier;

            if (string.IsNullOrWhiteSpace(traceIdentifier))
            {
                return await View(new List<T>(), typeof(TraceLogList));
            }

            return await View((await _logRepository
                .RequestTraceAsync(log))
                .OrderBy(x => x.LongDate).ToList(), typeof(TraceLogList));
        }
    }
}
