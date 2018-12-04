using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace NlogDashboard
{
    public class NlogDashboardOptions
    {
        internal string ConnetionString { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public TimeSpan SyncTime { get; set; }


        internal bool Authorzation { get; set; }

        public string PathMatch { get; set; }

        internal List<IAuthorizeData> AuthorizeDatas { get; set; }

        public void UseAuthorization(List<AuthorizeAttribute> authorizeAttributes)
        {
            Authorzation = true;
            AuthorizeDatas = new List<IAuthorizeData>();
            AuthorizeDatas.AddRange(authorizeAttributes);
        }


        public NlogDashboardOptions()
        {
            PathMatch = "/NlogDashboard";
            SyncTime = TimeSpan.FromSeconds(5);
        }

        public void UseDataBase(string connectionString)
        {
            ConnetionString = connectionString;
        }

    }
}

