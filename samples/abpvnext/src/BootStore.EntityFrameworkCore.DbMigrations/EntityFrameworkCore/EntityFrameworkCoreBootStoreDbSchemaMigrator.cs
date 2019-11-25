using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BootStore.Data;
using Volo.Abp.DependencyInjection;

namespace BootStore.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreBootStoreDbSchemaMigrator 
        : IBootStoreDbSchemaMigrator, ITransientDependency
    {
        private readonly BootStoreMigrationsDbContext _dbContext;

        public EntityFrameworkCoreBootStoreDbSchemaMigrator(BootStoreMigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}