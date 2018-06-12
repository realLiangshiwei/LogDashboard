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
     
            ViewBag.unique = await Conn.QueryFirstAsync<long>(
                "select count(b.count) from (select  count(distinct Exception) count from log where Exception!='' group by Exception) b");

            ViewBag.allCount = await Conn.QueryFirstAsync<long>("select count(id) from log");

            var now = DateTime.Now;

            var today = now.ToShortDateString();
            ViewBag.todayCount =
                await Conn.QueryFirstAsync<long>($"select count(id) from log where longdate>='{today}' and longdate<='{today + " 23:59"}'");

            var hour = now.AddHours(-1);
            ViewBag.hourCount =
                await Conn.QueryFirstAsync<long>(
                    $"select count(id) from log where longdate>='{hour}' and longdate<'{now}'");

            return await View(result);
        }


        public async Task<string> Searchlog(SearchlogInput input)
        {
            var result = await Conn.QueryAsync(BuildSql(input));
            return await View(result, "Views.Dashboard.LogList.cshtml");
        }

        public async Task<string> LogInfo(EnttiyDto input)
        {
            return await View();
        }

        public async Task<string> GetException(EnttiyDto input)
        {
            var result = await Conn.QueryFirstAsync<string>($"select exception from log where id = {input.Id}");
            return result;
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
