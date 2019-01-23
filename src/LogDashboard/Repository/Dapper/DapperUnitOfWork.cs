using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LogDashboard.Repository.Dapper
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _conn;

        public DapperUnitOfWork(SqlConnection conn)
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

        public SqlConnection GetConnection()
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
