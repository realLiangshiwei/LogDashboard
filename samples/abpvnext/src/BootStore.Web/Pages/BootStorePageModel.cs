using BootStore.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BootStore.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class BootStorePageModel : AbpPageModel
    {
        protected BootStorePageModel()
        {
            LocalizationResourceType = typeof(BootStoreResource);
        }
    }
}