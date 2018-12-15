using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;

namespace NLogDashboard
{
    public static class NLogDashboardServiceCollectionExtensions
    {
        public static IServiceCollection AddNLogDashboard(this IServiceCollection services, Action<NLogDashboardOptions> func)
        {
            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(NLogDashboardMiddleware))
                .UseMemoryCachingProvider()
                .Build());

            var options = new NLogDashboardOptions();
            func(options);

            services.AddSingleton(options);
           
            services.AddTransient(provider => new SqlConnection(options.ConnectionString));

            return services;
        }
    }
}
