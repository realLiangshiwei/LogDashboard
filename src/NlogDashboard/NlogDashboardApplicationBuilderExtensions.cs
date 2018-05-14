using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NlogDashboard
{
    public static class NlogDashboardApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNlogDashboard(
            this IApplicationBuilder builder, string pathMatch = "/NlogDashboard")
        {
            var options = builder.ApplicationServices.GetService<NlogDashboardOptions>();

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }


            return builder.Map(pathMatch, app => { app.UseMiddleware<NlogDashboardMiddleware>(); });
        }
    }
}
