using System;
using System.Linq;
using System.Threading.Tasks;
using LogDashboard.Models;
using LogDashboard.Repository;

namespace LogDashboard.Handle.LogChart
{
    public class MonthLogChart : ILogChart
    {
        public async Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
        {
            var now = DateTime.Now;
            var days = DateTime.DaysInMonth(now.Year, now.Month);
            var day = now.Day;
            var output = new GetLogChartsOutput(days);
            var date = new DateTime(now.Year, now.Month, 1);

            var list = await repository.GetLevelCount(ChartDataType.Month, date, now);

            for (var i = 0; i < days; i++)
            {
                if (i > day)
                {
                    output.All[i] = 0;
                    output.Error[i] = 0;
                    output.Info[i] = 0;
                    output.Debug[i] = 0;
                    output.Fatal[i] = 0;
                    output.Trace[i] = 0;
                    output.Warn[i] = 0;
                }
                else
                {
                    var dayTime = date.AddDays(i);

                    var thisList = list.Where(p => DateTime.Parse(p.LongDate) == dayTime);

                    output.All[i] = thisList.Sum(p => p.Count);
                    output.Error[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Error).Sum(p => p.Count);
                    output.Info[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Info).Sum(p => p.Count);
                    output.Debug[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Debug).Sum(p => p.Count);
                    output.Fatal[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Fatal).Sum(p => p.Count);
                    output.Trace[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Trace).Sum(p => p.Count);
                    output.Warn[i] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Warn).Sum(p => p.Count);
                }
            }
            return output;
        }

        private async Task<int> CountAsync<T>(string level, IRepository<T> repository, DateTime dayTime) where T : class, ILogModel
        {
            return await repository.CountAsync(x => x.LongDate >= dayTime && x.LongDate <= dayTime.AddHours(23).AddMinutes(59).AddSeconds(59) && x.Level == level);
        }

    }
}
