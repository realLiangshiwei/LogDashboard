using LogDashboard.Authorization;
using LogDashboard.Extensions;
using LogDashboard.Route;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogDashboard
{
    public class LogdashboardAccountAuthorizeFilter : ILogDashboardAuthorizationFilter
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 登陆过期时间，默认24小时
        /// </summary>
        public int LoginExpireHour { get; set; }

        public LogdashboardAccountAuthorizeFilter(string userName, string password, int loginExpireHour = 24)
        {
            UserName = userName;
            Password = password;
            LoginExpireHour = loginExpireHour;
        }

        public bool Authorization(LogDashboardContext context)
        {
            bool isValidAuthorize = false;

            if (context.HttpContext.Request != null && context.HttpContext.Request.Cookies != null)
            {
                context.HttpContext.Request.Cookies.TryGetValue(LogDashboardConsts.CookieTokenKey, out var token);
                context.HttpContext.Request.Cookies.TryGetValue(LogDashboardConsts.CookieTimestampKey, out var timestamp);
                if (double.TryParse(timestamp, out var time) &&
                    time <= DateTime.Now.ToUnixTimestamp() &&
                    time > DateTime.Now.AddDays(-LoginExpireHour).ToUnixTimestamp())
                {
                    var tokenValue = $"{UserName}&&{Password}&&{timestamp}".ToMD5();
                    isValidAuthorize = tokenValue == token;
                }
            }

            if (!isValidAuthorize)
            {
                var loginRouteUrl = LogDashboardRoutes.Routes.GetLoginRoute().Key;
                if (loginRouteUrl.ToLower() != context.HttpContext.Request.Path.Value.ToLower())
                {
                    var loginPath = $"{context.Options.PathMatch}{loginRouteUrl}";
                    context.HttpContext.Response.Redirect(loginPath);
                }
                else
                {
                    isValidAuthorize = true;
                }
            }

            return isValidAuthorize;
        }
    }
}
