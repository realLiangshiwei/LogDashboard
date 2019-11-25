using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BootStore.Data
{
    /* This is used if database provider does't define
     * IBootStoreDbSchemaMigrator implementation.
     */
    public class NullBootStoreDbSchemaMigrator : IBootStoreDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}