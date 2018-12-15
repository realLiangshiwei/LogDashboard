using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NLogDashboard
{
    public static class NLogDashboardApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNLogDashboard(
            this IApplicationBuilder builder, string pathMatch = "/NLogDashboard")
        {
            var options = builder.ApplicationServices.GetService<NLogDashboardOptions>();

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Map(pathMatch, app => { app.UseMiddleware<NLogDashboardMiddleware>(); });
        }
    }
}
