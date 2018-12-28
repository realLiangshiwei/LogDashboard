using System;
using DapperExtensions.Mapper;

namespace NLogDashboard.Repository.Dapper
{
    [Serializable]
    public sealed class LogModelMapper<T> : ClassMapper<T> where T : class
    {

        public LogModelMapper()
        {
            Table(NLogDashboardContext.StaticOptions.LogTableName);
            AutoMap();
        }

    }
}
