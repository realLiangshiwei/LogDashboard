using Volo.Abp.Settings;

namespace BootStore.Settings
{
    public class BootStoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BootStoreSettings.MySetting1));
        }
    }
}
