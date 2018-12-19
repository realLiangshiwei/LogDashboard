using System;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using NLogDashboard.Model;
using NLogDashboard.Repository;

namespace NLogDashboard.Handle
{
    public class DashboardHandle : NlogNLogDashboardHandleBase
    {
        private readonly IRepository<ILogModel> _logRepository;

        public DashboardHandle(
            IServiceProvider serviceProvider,
            IRepository<ILogModel> logRepository) : base(serviceProvider)
        {
            _logRepository = logRepository;
            //_exceptionDetailsProvider = new ExceptionDetailsProvider(new PhysicalFileProvider(AppContext.BaseDirectory), 6);
        }

        public async Task<string> Home()
        {
            var result = _logRepository.GetPageList(x => true, 1, 10, new Sort { Ascending = false, PropertyName = "longDate" });

            ViewBag.unique = _logRepository.GetList(x => true).GroupBy(x => x.Message).Count(x => x.Count() == 1);
            ViewBag.allCount = _logRepository.Count(x => true);

            var now = DateTime.Now;

            ViewBag.todayCount = _logRepository.Count(x =>
                   x.LongDate.Date >= now.Date && x.LongDate <= now.Date.AddHours(23).AddMinutes(59));

            var hour = now.AddHours(-1);
            ViewBag.hourCount = _logRepository.Count(x => x.LongDate >= hour && x.LongDate <= now);

            return await View(result);
        }


        //public async Task<string> SearchLog(SearchlogInput input)
        //{
        //    var result = await Conn.QueryAsync(BuildSql(input));
        //    return await View(result, "Views.Dashboard.LogList.cshtml");
        //}

        public async Task<string> LogInfo(EnttiyDto input)
        {
            var log = _logRepository.FirstOrDefault(x => x.Id == input.Id); ;
            return await View(log);
        }

        public  string GetException(EnttiyDto input)
        {
            var result = _logRepository.FirstOrDefault(x => x.Id == input.Id).Exception;
            return result;
        }
    }
}
