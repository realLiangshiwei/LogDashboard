using System.Data.SqlClient;

namespace NLogDashboard.Handle
{
    public interface INLogDashboardHandle
    {
        NLogDashboardContext Context { get; }

        SqlConnection Conn { get; }
    }
}
