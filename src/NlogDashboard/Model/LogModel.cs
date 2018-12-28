using System;
using DapperExtensions.Mapper;

namespace LogDashboard.Model
{

    public class LogModel : ILogModel
    {
        public int Id { get; set; }

        public DateTime LongDate { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Exception { get; set; }
    }

}
