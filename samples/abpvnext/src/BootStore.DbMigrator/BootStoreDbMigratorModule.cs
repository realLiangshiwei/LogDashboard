using BootStore.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace BootStore.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BootStoreEntityFrameworkCoreDbMigrationsModule),
        typeof(BootStoreApplicationContractsModule)
        )]
    public class BootStoreDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
