using System.Data.Common;
using System.Threading.Tasks;

namespace LogDashboard.Repository.Dapper
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly DbConnection _conn;

        public DapperUnitOfWork(DbConnection conn)
        {
            _conn = conn;
        }

        public async Task Open()
        {
            await _conn.OpenAsync();
        }

        public void Close()
        {
            _conn.Close();
        }

        public DbConnection GetConnection()
        {
            return _conn;
        }

        public void Dispose()
        {
            Close();
            _conn?.Dispose();
        }
    }
}
