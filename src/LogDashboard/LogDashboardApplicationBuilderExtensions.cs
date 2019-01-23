using Microsoft.AspNetCore.Builder;

namespace LogDashboard
{
#if NETSTANDARD2_0
        public static class LogDashboardApplicationBuilderExtensions
        {
            public static IApplicationBuilder UseLogDashboard(
                this IApplicationBuilder builder, string pathMatch = "/LogDashboard")
            {
                return builder.Map(pathMatch, app => { app.UseMiddleware<LogDashboardMiddleware>(); });
            }
        }
#endif

}
