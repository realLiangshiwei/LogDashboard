using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using DapperExtensions.Mapper;
using Microsoft.Extensions.DependencyInjection;
using NLogDashboard.Handle;
using NLogDashboard.NLogDashboardBuilder;
using NLogDashboard.Repository;
using NLogDashboard.Repository.Dapper;
using NLogDashboard.Route;
using RazorLight;

namespace NLogDashboard
{
    public static class NLogDashboardServiceCollectionExtensions
    {
        public static INlogDashboardBuilder AddNLogDashboard(this IServiceCollection services, Action<NLogDashboardOptions> func = null)
        {
            var builder = new DefaultNlogDashboardBuilder(services);

            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(NLogDashboardMiddleware))
                .UseMemoryCachingProvider()
                .Build());

            var options = new NLogDashboardOptions();
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
                builder.Services.AddTransient(typeof(IRepository<>), typeof(DatabaseRepository<>));
            }
            else
            {
                builder.Services.AddTransient(typeof(IRepository<>), typeof(FileRepository<>));
            }



            RegisterHandle(services, options);

            return builder;
        }


        private static void RegisterHandle(IServiceCollection services, NLogDashboardOptions opts)
        {
            var handles = Assembly.GetAssembly(typeof(NLogDashboardRoute)).GetTypes()
                .Where(x => typeof(NlogNLogDashboardHandleBase).IsAssignableFrom(x) && x != typeof(NlogNLogDashboardHandleBase));

            foreach (var handle in handles)
            {
                services.AddTransient(handle.MakeGenericType(opts.LogModelType));
            }
        }
    }
}
