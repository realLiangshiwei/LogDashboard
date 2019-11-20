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
                    output.Info[i] = 0;
                    output.Debug[i] = 0;
                    output.Fatal[i] = 0;
                    output.Trace[i] = 0;
                    output.Warn[i] = 0;
                }
                else
                {
                    var startTime = date.AddHours(i);
                    output.All[i] = await repository.CountAsync(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59).AddSeconds(59));
                    output.Error[i] = await CountAsync(LogLevel.ERROR, repository, startTime, i);
                    output.Info[i] = await CountAsync(LogLevel.INFO, repository, startTime, i);
                    output.Debug[i] = await CountAsync(LogLevel.DEBUG, repository, startTime, i);
                    output.Fatal[i] = await CountAsync(LogLevel.FATAL, repository, startTime, i);
                    output.Trace[i] = await CountAsync(LogLevel.TRACE, repository, startTime, i);
                    output.Warn[i] = await CountAsync(LogLevel.WARN, repository, startTime, i);
                }
            }
            return output;
        }

        private async Task<int> CountAsync<T>(LogLevel level, IRepository<T> repository, DateTime startTime, int i) where T : class, ILogModel
        {
            return await repository.CountAsync(x => x.LongDate >= startTime && x.LongDate <= startTime.AddMinutes(59).AddSeconds(59) && x.Level == level);
        }
    }
}
