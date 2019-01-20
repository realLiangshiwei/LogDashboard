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
            var output = new GetLogChartsOutput(7);
            for (var i = 0; i < 7; i++)
            {
                if (i > dayOfWeek || (i != dayOfWeek && dayOfWeek == 0))
                {
                    output.All[i == 0 ? 6 : i == 6 ? 0 : i] = 0;
                    output.Error[i == 0 ? 6 : i == 6 ? 0 : i] = 0;
                }
                else
                {
                    var day = now.AddDays(i - dayOfWeek);
                    var weeHours = now.AddDays(i - dayOfWeek).Date.AddHours(23).AddMinutes(59);
                    output.All[i == 0 ? 6 : i == 6 ? 0 : i] = await repository.Count(x => x.LongDate >= day.Date && x.LongDate <= weeHours);
                    output.Error[i == 0 ? 6 : i == 6 ? 0 : i] = await repository.Count(x => x.LongDate >= day.Date && x.LongDate <= weeHours && x.Level == "ERROR");
                }
            }

            return output;
        }
    }
}
