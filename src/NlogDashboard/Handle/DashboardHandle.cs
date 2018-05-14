using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace NlogDashboard.Handle
{
    public class DashboardHandle : NlogNlogDashboardHandleBase
    {


        public async Task<string> Home()
        {
            var result = await Conn.QueryAsync("select * from log order by id desc offset 0 rows fetch next 20 rows only");
            foreach (var o in result)
            {
                
            }
            return await View();
        }

        public DashboardHandle(NlogDashboardContext context, SqlConnection conn) : base(context, conn)
        {
        }
    }
}
