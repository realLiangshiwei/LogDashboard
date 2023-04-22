using LogDashboard;
using LogDashboard.Authorization.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NetCoreEmptyTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogDashboard(opt => opt.AddAuthorizationFilter(new LogDashboardBasicAuthFilter("admin", "admin")));
            //services.AddLogDashboard(opt => opt.AddAuthorizationFilter(new LogdashboardAccountAuthorizeFilter("admin", "123qwe", 1)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 静态文件中间件
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogDashboard();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
