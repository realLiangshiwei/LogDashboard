using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using DapperExtensions.Sql;
using LogDashboard.Cache;
using LogDashboard.LogDashboardBuilder;
using LogDashboard.Handle;
using LogDashboard.Repository;
using LogDashboard.Repository.Dapper;
using LogDashboard.Repository.File;
using LogDashboard.Route;
using LogDashboard.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LogDashboard
{
    public static class LogDashboardServiceCollectionExtensions
    {


        public static ILogDashboardBuilder AddLogDashboard(this IServiceCollection services, Action<LogDashboardOptions> func = null)
        {
            var builder = new DefaultLogDashboardBuilder(services);

            RegisterServices(services, func);

            return builder;
        }


        private static void RegisterServices(IServiceCollection services, Action<LogDashboardOptions> func = null, Assembly currentAssembly = null)
        {


            services.AddSingleton(typeof(ILogDashboardCacheManager<>), typeof(InMemoryLogDashboardCacheManager<>));

            // options
            var options = new LogDashboardOptions();
            func?.Invoke(options);

            services.AddSingleton(options);

            if (options.DatabaseSource)
            {
                DapperExtensions.DapperAsyncExtensions.DefaultMapper = typeof(LogModelMapper<>);
                DapperExtensions.DapperExtensions.DefaultMapper = typeof(LogModelMapper<>);
                DapperExtensions.DapperAsyncExtensions.SqlDialect = options.SqlDialect;
                DapperExtensions.DapperExtensions.SqlDialect = options.SqlDialect;

                if (options.DbConnectionFactory == null)
                {
                    throw new ArgumentNullException(nameof(options.DbConnectionFactory));
                }

                services.AddTransient(typeof(DbConnection), provider => options.DbConnectionFactory.Invoke());

                services.AddTransient(typeof(IRepository<>), typeof(DapperRepository<>));

                services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            }
            else
            {
                services.AddTransient(typeof(IRepository<>), typeof(FileRepository<>)); ;

                services.AddScoped(typeof(IUnitOfWork), typeof(FileUnitOfWork<>).MakeGenericType(options.LogModelType));
            }


            //register Handle
            RegisterHandle(services, options);

            //register Views
            RegisterViews(services);
        }

        private static void RegisterHandle(IServiceCollection services, LogDashboardOptions opts)
        {
            var handles = Assembly.GetAssembly(typeof(LogDashboardRoute)).GetTypes()
                .Where(x => typeof(LogDashboardHandleBase).IsAssignableFrom(x) && x != typeof(LogDashboardHandleBase));

            foreach (var handle in handles)
            {
                services.AddTransient(handle.MakeGenericType(opts.LogModelType));
            }
        }

        private static void RegisterViews(IServiceCollection services)
        {
            var views = Assembly.GetAssembly(typeof(LogDashboardRoute)).GetTypes()
                .Where(x => typeof(RazorPage).IsAssignableFrom(x) && x != typeof(RazorPage));

            foreach (var view in views)
            {
                services.AddTransient(view);
            }
        }


    }
}