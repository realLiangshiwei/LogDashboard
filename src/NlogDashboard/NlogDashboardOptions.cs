using System;

namespace NlogDashboard
{
    public class NlogDashboardOptions
    {
        internal string ConnetionString { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public TimeSpan SyncTime { get; set; }

        internal string Name { get; set; }

        internal string Password { get; set; }

        internal bool UseAuthorzation { get; set; }

        public string PathMatch { get; set; }

        public void UseAuthorization(string name, string password)
        {
            Name = name;
            Password = password;
            UseAuthorzation = true;
        }


        public NlogDashboardOptions()
        {
            PathMatch = "/NlogDashboard";
            SyncTime = TimeSpan.FromSeconds(5);
        }

        public void UseDataBaseSource(string connectionString)
        {
            ConnetionString = connectionString;
        }

    }
}
