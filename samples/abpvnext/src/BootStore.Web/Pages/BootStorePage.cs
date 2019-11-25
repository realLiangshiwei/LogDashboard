using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using BootStore.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BootStore.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits BootStore.Web.Pages.BootStorePage
     */
    public abstract class BootStorePage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BootStoreResource> L { get; set; }
    }
}
