using System;
using System.Linq;
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

            var list = await repository.GetLevelCount(ChartDataType.Hour, hourTime, now);

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
                    var thisList = list.Where(p => DateTime.Parse(p.LongDate).Minute == i);
                    output.All[i / 10] = thisList.Sum(p => p.Count);
                    output.Error[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Error).Sum(p => p.Count);
                    output.Info[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Info).Sum(p => p.Count);
                    output.Debug[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Debug).Sum(p => p.Count);
                    output.Fatal[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Fatal).Sum(p => p.Count);
                    output.Trace[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Trace).Sum(p => p.Count);
                    output.Warn[i / 10] = thisList.Where(p => p.Level.ToUpper() == LogLevelConst.Warn).Sum(p => p.Count);
                }
            }
            return output;
        }
    }
}
