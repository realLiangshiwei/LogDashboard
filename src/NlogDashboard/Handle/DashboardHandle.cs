using System;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using NLogDashboard.Model;
using NLogDashboard.Repository;

namespace NLogDashboard.Handle
{
    public class DashboardHandle<T> : NlogNLogDashboardHandleBase where T : class, ILogModel
    {
        private readonly IRepository<T> _logRepository;

        public DashboardHandle(
            IServiceProvider serviceProvider,
            IRepository<T> logRepository) : base(serviceProvider)
        {
            _logRepository = logRepository;
            //_exceptionDetailsProvider = new ExceptionDetailsProvider(new PhysicalFileProvider(AppContext.BaseDirectory), 6);
        }

        public async Task<string> Home()
        {
            var result = _logRepository.GetPageList(1, 10, sorts: new Sort { Ascending = false, PropertyName = "longDate" });

            ViewBag.unique = _logRepository.GetList().GroupBy(x => x.Message).Count(x => x.Count() == 1);
            ViewBag.allCount = _logRepository.Count();

            var now = DateTime.Now;
            var weeHours = now.Date.AddHours(23).AddMinutes(59);
            ViewBag.todayCount = _logRepository.Count(x => x.LongDate >= now.Date && x.LongDate <= weeHours);

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

        public string GetException(EnttiyDto input)
        {
            var result = _logRepository.FirstOrDefault(x => x.Id == input.Id).Exception;
            return result;
        }
    }
}
