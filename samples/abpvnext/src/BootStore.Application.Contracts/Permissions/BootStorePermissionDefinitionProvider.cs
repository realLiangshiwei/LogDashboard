using BootStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BootStore.Permissions
{
    public class BootStorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BootStorePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(BootStorePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BootStoreResource>(name);
        }
    }
}
