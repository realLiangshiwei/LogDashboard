using BootStore.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BootStore.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class BootStoreController : AbpController
    {
        protected BootStoreController()
        {
            LocalizationResource = typeof(BootStoreResource);
        }
    }
}