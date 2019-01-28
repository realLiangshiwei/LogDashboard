using System;
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
            for (var i = 0; i < days; i++)
            {
                if (i > day)
                {
                    output.All[i] = 0;
                    output.Error[i] = 0;
                }
                else
                {
                    var dayTime = date.AddDays(i);
                    output.All[i] = await repository.CountAsync(x => x.LongDate >= dayTime && x.LongDate <= dayTime.AddHours(23).AddMinutes(59).AddSeconds(59));
                    output.Error[i] = await repository.CountAsync(x => x.LongDate >= dayTime && x.LongDate <= dayTime.AddHours(23).AddMinutes(59).AddSeconds(59) && x.Level == "ERROR");
                }
            }
            return output;
        }

    }
}
