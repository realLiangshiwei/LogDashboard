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

        public (string, string) GetCookieValue(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(TokenKey, out var token);
            context.Request.Cookies.TryGetValue(TimestampKey, out var timestamp);
            return (token, timestamp);
        }


        public void SetCookieValue(HttpContext context, LogdashboardAccountAuthorizeFilter filter)
        {
            var timestamp = DateTime.Now.ToUnixTimestamp().ToString();
            var token = $"{Secure(filter)}&&{timestamp}".ToMD5();
            context.Response.Cookies.Append(TokenKey, token, new CookieOptions() { Expires = DateTime.Now.Add(Expire) });
            context.Response.Cookies.Append(TimestampKey, timestamp, new CookieOptions() { Expires = DateTime.Now.Add(Expire) });
        }
    }
}

