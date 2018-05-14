using System.Data.SqlClient;

namespace NlogDashboard.Handle
{
    public interface INlogDashboardHandle
    {
        NlogDashboardContext Context { get; }

        SqlConnection Conn { get; }
    }
}
