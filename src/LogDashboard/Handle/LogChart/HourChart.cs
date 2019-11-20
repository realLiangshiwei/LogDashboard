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
                    output.Info[i / 10] = 0;
                    output.Debug[i / 10] = 0;
                    output.Fatal[i / 10] = 0;
                    output.Trace[i / 10] = 0;
                    output.Warn[i / 10] = 0;
                }
                else
                {
                    output.All[i / 10] = await repository.CountAsync(x => x.LongDate >= hourTime.AddMinutes(i) && x.LongDate <= hourTime.AddMinutes(i + 9).AddSeconds(59));
                    output.Error[i / 10] = await CountAsync(LogLevel.ERROR, repository, hourTime, i);
                    output.Info[i / 10] = await CountAsync(LogLevel.INFO, repository, hourTime, i);
                    output.Debug[i / 10] = await CountAsync(LogLevel.DEBUG, repository, hourTime, i);
                    output.Fatal[i / 10] = await CountAsync(LogLevel.FATAL, repository, hourTime, i);
                    output.Trace[i / 10] = await CountAsync(LogLevel.TRACE, repository, hourTime, i);
                    output.Warn[i / 10] = await CountAsync(LogLevel.WARN, repository, hourTime, i);
                }
            }
            return output;
        }

        private async Task<int> CountAsync<T>(LogLevel level, IRepository<T> repository, DateTime hourTime, int i) where T : class, ILogModel
        {
            return await repository.CountAsync(x => x.LongDate >= hourTime.AddMinutes(i) && x.LongDate <= hourTime.AddMinutes(i + 9).AddSeconds(59) && x.Level == level);
        }
    }
}
