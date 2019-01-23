using System;
using System.Threading.Tasks;

namespace LogDashboard.Repository
{
    public interface IUnitOfWork :IDisposable
    {
        Task Open();

        void Close();
    }
}
