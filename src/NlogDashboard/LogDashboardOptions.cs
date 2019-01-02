using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using LogDashboard.Model;

namespace LogDashboard
{
    public class LogDashboardOptions
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

        public void UseAuthorization(params AuthorizeAttribute[] authorizeAttributes)
        {
            Authorization = true;
            AuthorizeData = new List<IAuthorizeData>();

            if (authorizeAttributes != null)
            {
                AuthorizeData.AddRange(authorizeAttributes);
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
            CustomPropertyInfos = new List<PropertyInfo>();
            FileSource = true;
            NogConfig = "NLog.config";
            PathMatch = "/LogDashboard";
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

