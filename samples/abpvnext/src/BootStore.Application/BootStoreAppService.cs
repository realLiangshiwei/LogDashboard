using System;
using System.Collections.Generic;
using System.Text;
using BootStore.Localization;
using Volo.Abp.Application.Services;

namespace BootStore
{
    /* Inherit your application services from this class.
     */
    public abstract class BootStoreAppService : ApplicationService
    {
        protected BootStoreAppService()
        {
            LocalizationResource = typeof(BootStoreResource);
        }
    }
}
