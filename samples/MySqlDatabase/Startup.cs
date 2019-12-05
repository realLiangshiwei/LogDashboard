using DapperExtensions.Sql;
using LogDashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace MySqlDatabase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogDashboard(opt =>
            {
                opt.CustomLogModel<CustomLogModel>();
                opt.UseDataBase(() => new MySqlConnection(Configuration.GetConnectionString("DefaultConnection")), sqlDialect: new MySqlDialect());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogDashboard();

            app.Run(async (context) =>
            {
                var log = app.ApplicationServices.GetService<ILogger<Startup>>();
                log.LogInformation("info message");
                log.LogDebug("debug message");
                log.LogError("error message");
                log.LogTrace("trace message");
                log.LogWarning("warn message");
                await context.Response.WriteAsync("Hello World!");
            });


        }
    }
}
