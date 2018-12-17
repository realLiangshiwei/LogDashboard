using Microsoft.Extensions.DependencyInjection;

namespace NLogDashboard.NLogDashboardBuilder
{
    public interface INlogDashboardBuilder
    {
        IServiceCollection Services { get; }
    }
}
