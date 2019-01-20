using System;
using System.Threading.Tasks;
using LogDashboard.Models;
using LogDashboard.Repository;

namespace LogDashboard.Handle.LogChart
{
    public class HourChart : ILogChart
    {
        public async Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel
        {
            var now = DateTime.Now;
            var minute = now.Minute;
            var hourTime = DateTime.Now.Date.AddHours(now.Hour);
            var output = new GetLogChartsOutput(6);
            for (var i = 0; i < 60; i += 10)
            {
                if (i > minute)
                {
                    output.All[i / 10] = 0;
                    output.Error[i / 10] = 0;
                }
                else
                {
                    output.All[i / 10] = await repository.Count(x => x.LongDate >= hourTime.AddMinutes(i) && x.LongDate <= hourTime.AddMinutes(i + 9));
                    output.Error[i / 10] = await repository.Count(x => x.LongDate >= hourTime.AddMinutes(i) && x.LongDate <= hourTime.AddMinutes(i + 9) && x.Level == "ERROR");
                }
            }
            return output;
        }
    }
}
