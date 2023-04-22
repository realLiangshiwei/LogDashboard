using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DapperExtensions.Sql;
using LogDashboard.Authorization;
using LogDashboard.Extensions;
using LogDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace LogDashboard
{
    public class LogDashboardCookieOptions
    {
        public TimeSpan Expire { get; set; }

        public string TokenKey { get; set; }

        public string TimestampKey { get; set; }

        public Func<LogdashboardAccountAuthorizeFilter, string> Secure { get; set; }

        public LogDashboardCookieOptions()
        {
            Expire = TimeSpan.FromDays(1);
            TokenKey = "LogDashboard.CookieKey";
            TimestampKey = "LogDashboard.Timestamp";
            Secure = (filter) => $"{filter.UserName}&&{filter.Password}";
        }
    }
}

