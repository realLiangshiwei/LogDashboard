using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RazorLight;

namespace NlogDashboard
{
    public static class NlogDashboardServiceCollectionExtensions
    {
        public static IServiceCollection AddNlogDashboard(this IServiceCollection services)
        {
            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(NlogDashboardRoute))
                .UseMemoryCachingProvider()
                .Build());

            return services;
        }
    }
}
