using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NlogDashboard;

namespace NetCoreEmptyTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNlogDashboard(opt =>
            {
                opt.UseAuthorization(new List<AuthorizeAttribute>
                {
                    new AuthorizeAttribute()
                });
                opt.UseDataBase("Server=localhost; Database=log.test.edu.jingshonline.net;Integrated Security=True;");
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

            app.UseNlogDashboard();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
