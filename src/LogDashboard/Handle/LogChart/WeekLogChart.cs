using System;
using System.Linq;
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

            var cweek = dayOfWeek == 0 ? 7 : dayOfWeek;

            var list = await repository.GetLevelCount(ChartDataType.Week, now.Date.AddDays(-cweek + 1), now);

            for (var i = 0; i < 7; i++)
            {
                if (i > dayOfWeek)
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
                    var thisList = list.Where(p => DateTime.Parse(p.LongDate) == (now.Date.AddDays(-cweek + i + 1)));

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
    }
}
