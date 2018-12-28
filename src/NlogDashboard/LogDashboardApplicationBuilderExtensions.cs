using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using LogDashboard;

namespace LogDashboard
{
    public static class LogDashboardApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLogDashboard(
            this IApplicationBuilder builder, string pathMatch = "/LogDashboard")
        {
            var options = builder.ApplicationServices.GetService<LogDashboardOptions>();

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Map(pathMatch, app => { app.UseMiddleware<LogDashboardMiddleware>(); });
        }
    }
}
