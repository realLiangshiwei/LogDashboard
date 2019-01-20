using System.Threading.Tasks;
using LogDashboard.Models;
using LogDashboard.Repository;

namespace LogDashboard.Handle.LogChart
{
    public interface ILogChart
    {
        Task<GetLogChartsOutput> GetCharts<T>(IRepository<T> repository) where T : class, ILogModel;
    }
}
