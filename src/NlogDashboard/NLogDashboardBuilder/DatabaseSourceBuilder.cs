using Microsoft.Extensions.DependencyInjection;
using NLogDashboard.Model;
using NLogDashboard.Repository;

namespace NLogDashboard.NLogDashboardBuilder
{
    public static class DatabaseSourceBuilder
    {
        public static INlogDashboardBuilder AddDatabaseSource(this INlogDashboardBuilder builder)
        {
            builder.Services.AddTransient<IRepository<ILogModel>, DatabaseRepository<LogModel>>();
            return builder;
        }

        public static INlogDashboardBuilder AddDatabaseSource<T>(this INlogDashboardBuilder builder) where T : ILogModel
        {
            builder.Services.AddTransient(typeof(IRepository<ILogModel>),
                typeof(DatabaseRepository<>).MakeGenericType(typeof(T)));
            return builder;
        }
    }
}
