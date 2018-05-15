using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using NlogDashboard.Model;

namespace NlogDashboard.Handle
{
    public class DashboardHandle : NlogNlogDashboardHandleBase
    {

        public DashboardHandle(NlogDashboardContext context, SqlConnection conn) : base(context, conn)
        {
        }

        public async Task<string> Home()
        {
            var result = await Conn.QueryAsync("select * from log order by id desc offset 0 rows fetch next 10 rows only");

            return await View(result);
        }


        public async Task<string> Searchlog(SearchlogInput input)
        {
            var result = await Conn.QueryAsync(BuildSql(input));

            return await View(result, true);
        }

        public string BuildSql(SearchlogInput input)
        {
            if (input.All)
            {
                return $"select * from log order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.ToDay)
            {
                var now = DateTime.Now.ToShortDateString();
                return
                    $"select * from log where longdate>={now} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.Hour)
            {
                var now = DateTime.Now.AddHours(-1);

                return $"select * from log where longDate>={now} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }

            if (input.Unique)
            {
                return
                    $"select b.* from log b where b.Message in(select Message from log a group by a.Message having count(a.Message) = 1) order by b.Id desc  offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";

            }

            if (!string.IsNullOrWhiteSpace(input.Level))
            {
                return
                    $"select * from log where level= {input.Level} order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
            }
            return $"select * from log order by id desc offset {(input.Page - 1) * input.PageSize} rows fetch next {input.PageSize} rows only";
        }
    }
}
