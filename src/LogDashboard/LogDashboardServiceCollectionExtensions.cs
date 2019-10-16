using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using LogDashboard.Cache;
using LogDashboard.LogDashboardBuilder;
using LogDashboard.Handle;
using LogDashboard.Repository;
using LogDashboard.Repository.Dapper;
using LogDashboard.Repository.File;
using LogDashboard.Route;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;

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
            // razor
            var razorBuilder = new RazorLightEngineBuilder();
            if (currentAssembly != null)
            {
                razorBuilder = razorBuilder.SetOperatingAssembly(currentAssembly);
            }

            services.AddSingleton<IRazorLightEngine>(razorBuilder
                .UseEmbeddedResourcesProject(typeof(LogDashboardMiddleware))
                .UseMemoryCachingProvider()
                .Build());

            services.AddSingleton(typeof(ILogDashboardCacheManager<>), typeof(InMemoryLogDashboardCacheManager<>));

            // options
            var options = new LogDashboardOptions();
            func?.Invoke(options);

            services.AddSingleton(options);

            if (options.DatabaseSource)
            {
                DapperExtensions.DapperAsyncExtensions.DefaultMapper = typeof(LogModelMapper<>);
                DapperExtensions.DapperExtensions.DefaultMapper = typeof(LogModelMapper<>);

                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                {
                    throw new ArgumentNullException(nameof(options.ConnectionString));
                }

                services.AddTransient(provider => new SqlConnection(options.ConnectionString));

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