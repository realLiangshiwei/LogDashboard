using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using LogDashboard.Handle;
using LogDashboard.LogDashboardBuilder;
using LogDashboard.Repository;
using LogDashboard.Repository.Dapper;
using LogDashboard.Repository.File;
using LogDashboard.Route;
using RazorLight;

namespace LogDashboard
{
    public static class LogDashboardServiceCollectionExtensions
    {
        public static ILogDashboardBuilder AddLogDashboard(this IServiceCollection services, Action<LogDashboardOptions> func = null)
        {
            var builder = new DefaultLogDashboardBuilder(services);

            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(LogDashboardMiddleware))
                .UseMemoryCachingProvider()
                .Build());

            var options = new LogDashboardOptions();
            func?.Invoke(options);

            services.AddSingleton(options);

            if (options.DatabaseSource)
            {
                DapperExtensions.DapperExtensions.DefaultMapper = typeof(LogModelMapper<>);

                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                {
                    throw new ArgumentNullException("ConnectionString Cannot be Null");
                }

                services.AddTransient(provider => new SqlConnection(options.ConnectionString));

                builder.Services.AddTransient(typeof(IRepository<>), typeof(DapperRepository<>));

                builder.Services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            }
            else
            {
                builder.Services.AddTransient(typeof(IRepository<>), typeof(FileRepository<>));               ;

                builder.Services.AddScoped(typeof(IUnitOfWork), typeof(FileUnitOfWork<>).MakeGenericType(options.LogModelType));
            }


            RegisterHandle(services, options);

            return builder;
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
    }
}
