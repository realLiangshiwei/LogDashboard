using System.Threading.Tasks;

namespace BootStore.Data
{
    public interface IBootStoreDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
