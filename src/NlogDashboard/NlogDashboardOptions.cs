using System;
using System.Collections.Generic;
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

        public void UseAuthorization(List<AuthorizeAttribute> authorizeAttributes)
        {
            Authorization = true;
            AuthorizeData = new List<IAuthorizeData>();
            AuthorizeData.AddRange(authorizeAttributes);
        }

        public void CustomLogModel<T>() where T : class, ILogModel
        {
            LogModelType = typeof(T);
        }

        public NLogDashboardOptions()
        {
            FileSource = true;
            NogConfig = "NLog.config";
            PathMatch = "/NLogDashboard";
            LogModelType = typeof(LogModel);
        }

        public void UseDataBase(string connectionString)
        {
            DatabaseSource = true;
            ConnectionString = connectionString;
        }

    }
}

