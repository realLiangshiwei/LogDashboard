using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace BootStore.Web
{
    [Dependency(ReplaceServices = true)]
    public class BootStoreBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "BootStore";
    }
}
