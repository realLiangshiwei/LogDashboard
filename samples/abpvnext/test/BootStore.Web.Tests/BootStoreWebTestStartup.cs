using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace BootStore
{
    public class BootStoreWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<BootStoreWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}