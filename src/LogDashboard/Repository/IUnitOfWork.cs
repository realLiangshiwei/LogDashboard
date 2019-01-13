using System;

namespace LogDashboard.Repository
{
    public interface IUnitOfWork :IDisposable
    {
        void Open();

        void Close();
    }
}
