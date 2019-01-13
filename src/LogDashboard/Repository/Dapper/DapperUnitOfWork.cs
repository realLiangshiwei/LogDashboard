using System.Data.SqlClient;

namespace LogDashboard.Repository.Dapper
{
    public class DapperUnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _conn;

        public DapperUnitOfWork(SqlConnection conn)
        {
            _conn = conn;
            Open();
        }

        public void Open()
        {
            _conn.Open();
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
