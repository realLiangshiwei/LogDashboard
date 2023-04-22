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

        public LogDashboardCookieOptions CookieOptions { get; set; }

        public LogdashboardAccountAuthorizeFilter(string userName, string password)
        {
            UserName = userName;
            Password = password;
            CookieOptions = new LogDashboardCookieOptions();
        }

        public LogdashboardAccountAuthorizeFilter(string userName, string password, LogDashboardCookieOptions cookieOptions)
        {
            UserName = userName;
            Password = password;
            CookieOptions = cookieOptions;
        }

        public bool Authorization(LogDashboardContext context)
        {
            bool isValidAuthorize = false;

            if (context.HttpContext.Request != null && context.HttpContext.Request.Cookies != null)
            {
                var (token, timestamp) = GetCookieValue(context.HttpContext);

                if (double.TryParse(timestamp, out var time) &&
                    time <= DateTime.Now.ToUnixTimestamp() &&
                    time > DateTime.Now.Add(-CookieOptions.Expire).ToUnixTimestamp())
                {
                    var tokenValue = $"{CookieOptions.Secure(this)}&&{timestamp}".ToMD5();
                    isValidAuthorize = tokenValue == token;
                }
            }

            //Rediect
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

        public (string, string) GetCookieValue(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(CookieOptions.TokenKey, out var token);
            context.Request.Cookies.TryGetValue(CookieOptions.TimestampKey, out var timestamp);
            return (token, timestamp);
        }

        public void SetCookieValue(HttpContext context)
        {
            var timestamp = DateTime.Now.ToUnixTimestamp().ToString();
            var token = $"{CookieOptions.Secure(this)}&&{timestamp}".ToMD5();
            context.Response.Cookies.Append(CookieOptions.TokenKey, token, new CookieOptions() { Expires = DateTime.Now.Add(CookieOptions.Expire) });
            context.Response.Cookies.Append(CookieOptions.TimestampKey, timestamp, new CookieOptions() { Expires = DateTime.Now.Add(CookieOptions.Expire) });
        }
    }
}
