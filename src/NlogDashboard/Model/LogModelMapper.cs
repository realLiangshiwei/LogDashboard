using System;
using System.Collections.Generic;
using System.Text;
using DapperExtensions.Mapper;

namespace NLogDashboard.Model
{
    [Serializable]
    public  class LogModelMapper : ClassMapper<LogModel>
    {

        public LogModelMapper()
        {
            Table("Log");
            AutoMap();
        }
    }
}
