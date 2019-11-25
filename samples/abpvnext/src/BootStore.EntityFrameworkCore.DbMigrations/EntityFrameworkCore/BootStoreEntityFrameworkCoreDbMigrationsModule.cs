using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BootStore.EntityFrameworkCore
{
    [DependsOn(
        typeof(BootStoreEntityFrameworkCoreModule)
        )]
    public class BootStoreEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BootStoreMigrationsDbContext>();
        }
    }
}
