using Microsoft.Extensions.DependencyInjection;

namespace LogDashboard.LogDashboardBuilder
{
    public interface ILogDashboardBuilder
    {
        IServiceCollection Services { get; }
    }
}
