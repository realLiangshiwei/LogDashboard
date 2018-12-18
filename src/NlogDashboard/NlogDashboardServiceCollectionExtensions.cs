using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using DapperExtensions.Mapper;
using Microsoft.Extensions.DependencyInjection;
using NLogDashboard.NLogDashboardBuilder;
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
                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                {
                    throw new ArgumentNullException("ConnectionString cannot be null");
                }
                services.AddTransient(provider => new SqlConnection(options.ConnectionString));
                builder.AddDatabaseSource();
            }
            else
            {
                builder.AddFileSource();
            }

            RegisterHandle(services);

            return builder;
        }


        private static void RegisterHandle(IServiceCollection services)
        {
            var handles = Assembly.GetAssembly(typeof(NLogDashboardRoute)).GetTypes()
                .Where(x => x.Name.EndsWith("Handle") && x.IsClass);

            foreach (var handle in handles)
            {
                services.AddTransient(handle);
            }
        }
    }
}
