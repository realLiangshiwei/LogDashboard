using Microsoft.Extensions.DependencyInjection;

namespace LogDashboard.LogDashboardBuilder
{
    public class DefaultLogDashboardBuilder : ILogDashboardBuilder
    {
        public DefaultLogDashboardBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
