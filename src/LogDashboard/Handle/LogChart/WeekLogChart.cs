using System;
using System.Threading.Tasks;
using LogDashboard.Models;
using LogDashboard.Repository;

namespace LogDashboard.Handle.LogChart
{
    public class WeekLogChart : ILogChart
    {
        public async Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
        {
            var now = DateTime.Now;
            var dayOfWeek = (int)now.DayOfWeek;
            dayOfWeek = dayOfWeek == 0 ? 6 : dayOfWeek == 6 ? 0 : dayOfWeek;
            var output = new GetLogChartsOutput(7);
            for (var i = 0; i < 7; i++)
            {
                if (i > dayOfWeek)
                {
                    output.All[i] = 0;
                    output.Error[i] = 0;
                }
                else
                {
  
                    var day = now.AddDays(0 - (dayOfWeek - i));
                    var weeHours = now.AddDays(i - dayOfWeek).Date.AddHours(23).AddMinutes(59);
                    output.All[i] = await repository.CountAsync(x => x.LongDate >= day.Date && x.LongDate <= weeHours);
                    output.Error[i ] = await repository.CountAsync(x => x.LongDate >= day.Date && x.LongDate <= weeHours && x.Level == "ERROR");
                }
            }

            return output;
        }
    }
}
