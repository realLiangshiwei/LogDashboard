using BootStore.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace BootStore
{
    [DependsOn(
        typeof(BootStoreEntityFrameworkCoreTestModule)
        )]
    public class BootStoreDomainTestModule : AbpModule
    {

    }
}