using Volo.Abp.Modularity;

namespace BootStore
{
    [DependsOn(
        typeof(BootStoreApplicationModule),
        typeof(BootStoreDomainTestModule)
        )]
    public class BootStoreApplicationTestModule : AbpModule
    {

    }
}