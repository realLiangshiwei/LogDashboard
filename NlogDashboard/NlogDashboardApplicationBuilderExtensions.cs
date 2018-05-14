using Microsoft.AspNetCore.Builder;

namespace NlogDashboard
{
    public static class NlogDashboardApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNlogDashboard(
            this IApplicationBuilder builder, string pathMatch = "/NlogDashboard")
        {
            return builder.Map(pathMatch, app => { app.UseMiddleware<NlogDashboardMiddleware>(); });
        }
    }
}
