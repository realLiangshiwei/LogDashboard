using Microsoft.Extensions.DependencyInjection;

namespace NLogDashboard.NLogDashboardBuilder
{
    public class DefaultNlogDashboardBuilder : INlogDashboardBuilder
    {
        public DefaultNlogDashboardBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
