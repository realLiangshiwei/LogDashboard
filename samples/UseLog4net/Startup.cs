using System;
using System.Collections.Generic;
using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Entities;

namespace UseLog4net
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogDashboard();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net(new Log4NetProviderOptions
            {
                PropertyOverrides =
                    new List<NodeInfo>
                    {
                        new NodeInfo { XPath = "/log4net/appender/file[last()]", Attributes = new Dictionary<string, string> { { "value", $"{AppContext.BaseDirectory}LogFiles/" } } }
                    }
            });
            app.UseLogDashboard();
            app.Run(async (context) =>
            {
                //ThreadContext.Properties["identity"] = context.TraceIdentifier;
                var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
                logger.LogInformation("来点日志");
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
