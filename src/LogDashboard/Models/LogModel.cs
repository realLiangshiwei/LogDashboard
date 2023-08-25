using System;
using DapperExtensions.Mapper;
using LogDashboard.Extensions;

namespace LogDashboard.Models
{

    public class LogModel : ILogModel
    {
        private string _level;
        public int Id { get; set; }

        public DateTime LongDate { get; set; }

        public string Level { get { return _level.ToUpper(); }  set { _level = value; } }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }
    }

}
