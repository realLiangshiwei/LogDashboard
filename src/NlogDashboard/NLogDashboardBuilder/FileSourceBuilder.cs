using Microsoft.Extensions.DependencyInjection;
using NLogDashboard.Model;
using NLogDashboard.Repository;

namespace NLogDashboard.NLogDashboardBuilder
{
    public static class FileSourceBuilder
    {
        public static INlogDashboardBuilder AddFileSource(this INlogDashboardBuilder builder)
        {
            builder.Services.AddTransient<IRepository<ILogModel>, FileRepository<LogModel>>();
            return builder;
        }

        public static INlogDashboardBuilder AddFileSource<T>(this INlogDashboardBuilder builder) where T : ILogModel
        {
            builder.Services.AddTransient(typeof(IRepository<ILogModel>),
                typeof(FileRepository<>).MakeGenericType(typeof(T)));
            return builder;
        }
    }
}
