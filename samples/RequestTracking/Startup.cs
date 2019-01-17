using LogDashboard;
using LogDashboard.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RequestTracking
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogDashboard(opt =>
            {
                opt.CustomLogModel<RequestTraceLogModel>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var log = app.ApplicationServices.GetService<ILogger<Startup>>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseLogDashboard();
            app.Run(async (context) =>
            {
                log.LogInformation("before write Hello world");

                await context.Response.WriteAsync("Hello World!");

                log.LogInformation("after write Hello world");
            });
        }
    }
}
