using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DapperExtensions.Sql;
using LogDashboard.Authorization;

using LogDashboard.Models;

namespace LogDashboard
{
    public class LogDashboardOptions
    {
        /// <summary>
        /// Default value : Log Dashboard
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Url match
        /// </summary>
        public string PathMatch { get; set; }

        public bool FileSource { get; set; }

        /// <summary>
        /// Log files path
        /// </summary>
        public string RootPath { get; set; }

        public bool DatabaseSource { get; set; }

        internal Func<DbConnection> DbConnectionFactory { get; set; }

        internal ISqlDialect SqlDialect { get; set; }

        internal Type LogModelType { get; set; }

        public TimeSpan CacheExpires { get; set; }

        internal List<ILogDashboardAuthorizationFilter> AuthorizationFiles { get; set; }

        internal List<PropertyInfo> CustomPropertyInfos { get; set; }

        internal string LogTableName { get; set; }

        /// <summary>
        /// file log field Delimiter
        /// </summary>
        public string FileFieldDelimiter { get; set; }

        /// <summary>
        /// file log end Delimiter
        /// </summary>
        public string FileEndDelimiter { get; set; }



        public void AddAuthorizationFilter(params ILogDashboardAuthorizationFilter[] filters)
        {
            if (filters != null)
            {
                AuthorizationFiles.AddRange(filters);
            }
        }

        public void CustomLogModel<T>() where T : class, ILogModel
        {
            LogModelType = typeof(T);

            CustomPropertyInfos = LogModelType.GetProperties().Where(x => !x.Name.Equals("LongDate", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Level", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Logger", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Message", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Exception", StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public LogDashboardOptions()
        {
            Brand = "Log Dashboard";
            CustomPropertyInfos = new List<PropertyInfo>();
            FileSource = true;
            FileFieldDelimiter = "||";
            FileEndDelimiter = "||end";
            PathMatch = "/LogDashboard";
            LogModelType = typeof(LogModel);
            AuthorizationFiles = new List<ILogDashboardAuthorizationFilter>();
            CacheExpires = TimeSpan.FromMinutes(5);
        }

        public void UseDataBase(Func<DbConnection> dbConnectionFactory, string tableName = "log", ISqlDialect sqlDialect = null)
        {
            LogTableName = tableName;
            DatabaseSource = true;
            FileSource = false;
            DbConnectionFactory = dbConnectionFactory;
            SqlDialect = sqlDialect ?? new SqlServerDialect();
        }

    }
}

