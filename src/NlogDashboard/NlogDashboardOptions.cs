using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using NLogDashboard.Model;

namespace NLogDashboard
{
    public class NLogDashboardOptions
    {
        public string NogConfig { get; set; }

        public bool Authorization { get; set; }

        public string PathMatch { get; set; }

        public bool FileSource { get; set; }

        public bool DatabaseSource { get; set; }

        public string ConnectionString { get; set; }

        internal Type LogModelType { get; set; }

        internal List<IAuthorizeData> AuthorizeData { get; set; }

        internal List<PropertyInfo> CustomPropertyInfos { get; set; }

        internal string LogTableName { get; set; }

        public void UseAuthorization(List<AuthorizeAttribute> authorizeAttributes)
        {
            Authorization = true;
            AuthorizeData = new List<IAuthorizeData>();
            AuthorizeData.AddRange(authorizeAttributes);
        }

        public void CustomLogModel<T>() where T : class, ILogModel
        {
            LogModelType = typeof(T);

            CustomPropertyInfos = LogModelType.GetProperties().Where(x => !x.Name.Equals("LongDate", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Logger", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Message", StringComparison.CurrentCultureIgnoreCase) &&
                                              !x.Name.Equals("Exception", StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public NLogDashboardOptions()
        {
            CustomPropertyInfos = new List<PropertyInfo>();
            FileSource = true;
            NogConfig = "NLog.config";
            PathMatch = "/NLogDashboard";
            LogModelType = typeof(LogModel);
        }

        public void UseDataBase(string connectionString, string tableName = "log")
        {
            LogTableName = tableName;
            DatabaseSource = true;
            ConnectionString = connectionString;
        }

    }
}

