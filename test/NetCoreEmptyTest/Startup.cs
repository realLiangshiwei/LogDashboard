using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NLogDashboard;

namespace NetCoreEmptyTest
{


    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddNLogDashboard(opt =>
            {
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 静态文件中间件
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseNLogDashboard();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
