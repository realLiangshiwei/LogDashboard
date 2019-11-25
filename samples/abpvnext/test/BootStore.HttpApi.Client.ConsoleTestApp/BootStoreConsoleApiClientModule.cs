using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace BootStore.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(BootStoreHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BootStoreConsoleApiClientModule : AbpModule
    {
        
    }
}
