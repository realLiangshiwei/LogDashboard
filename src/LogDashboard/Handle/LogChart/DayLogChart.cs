using System;
using System.Threading.Tasks;
using LogDashboard.Models;
using LogDashboard.Repository;

namespace LogDashboard.Handle.LogChart
{
    public class DayLogChart : ILogChart
    {
        public async Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
        {
            var now = DateTime.Now;
            var hour = now.Hour;

            var output = new GetLogChartsOutput(24);

            var date = now.Date;
            for (var i = 0; i < 24; i++)
            {
                if (i > hour)
                {
                    output.All[i] = 0;
                    output.Error[i] = 0;
                }
                else
                {
                    var startTime = date.AddHours(i);
                    output.All[i] = await repository.Count(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59));
                    output.Error[i] = await repository.Count(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59) && x.Level == "ERROR");
                }
            }
            return output;
        }
    }
}
