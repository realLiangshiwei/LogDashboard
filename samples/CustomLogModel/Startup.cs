using System;
using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace CustomLogModel
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            LogManager.Configuration.Variables["application"] = "CustomLogModel";
            LogManager.Configuration.Variables["requestMethod"] = "Get";
            services.AddLogDashboard(opt =>
            {
                opt.CustomLogModel<ApplicationLogModel>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogDashboard();
            app.Run(async (context) =>
            {
                var log = app.ApplicationServices.GetService<ILogger<Startup>>();
                log.LogInformation("info");
                log.LogDebug("debug <br> qaqa");
                log.LogTrace("trace");
                log.LogWarning("warn");
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
