using System;
using System.Threading.Tasks;
using LogDashboard;
using LogDashboard.Extensions;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NfxAspNetMvc.Startup))]

namespace NfxAspNetMvc
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapLogDashboard(typeof(Startup).Assembly, opt =>
            {
                opt.SetRootPath(AppContext.BaseDirectory);
            });

            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
