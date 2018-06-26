using System;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;

namespace NlogDashboard
{
    public static class NlogDashboardServiceCollectionExtensions
    {
        public static IServiceCollection AddNlogDashboard(this IServiceCollection services, Action<NlogDashboardOptions> func)
        {
            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(NlogDashboardMiddleware))
                .UseMemoryCachingProvider()
                .Build());

            var options = new NlogDashboardOptions();
            func(options);

            services.AddSingleton(options);

            services.AddTransient(provider => new SqlConnection(options.ConnetionString));

            return services;
        }
    }
}
