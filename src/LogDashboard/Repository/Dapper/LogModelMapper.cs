﻿using System;
using DapperExtensions.Mapper;

namespace LogDashboard.Repository.Dapper
{
    [Serializable]
    public sealed class LogModelMapper<T> : ClassMapper<T> where T : class
    {

        public LogModelMapper()
        {
            Schema(LogDashboardContext.StaticOptions.LogSchemaName);
            Table(LogDashboardContext.StaticOptions.LogTableName);
            AutoMap();
        }

    }
}
