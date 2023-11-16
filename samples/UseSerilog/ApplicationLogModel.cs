using LogDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseSerilog
{
    /// <summary>
    /// 自定义日志模型
    /// </summary>
    public class ApplicationLogModel : LogModel
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string MachineName { get; set; } = "";
        /// <summary>
        /// 环境用户名称
        /// </summary>
        public string EnvironmentUserName { get; set; } = "";
        /// <summary>
        /// 进程ID
        /// </summary>
        public string ProcessId { get; set; } = "";
        /// <summary>
        /// 线程ID
        /// </summary>
        public string ThreadId { get; set; } = "";
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIp { get; set; } = "";
        /// <summary>
        /// 客户端用户代理
        /// </summary>
        public string UserAgent { get; set; } = "";
        /// <summary>
        /// 企业ID
        /// </summary>
        public string Enterpriseid { get; set; } = "";
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestPath { get; set; } = "";
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod { get; set; } = "";
        /// <summary>
        /// 来源页面
        /// </summary>
        public string Referer { get; set; } = "";
        /// <summary>
        /// 用户账户ID
        /// </summary>
        public string UserAccountId { get; set; } = "";
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserInfoId { get; set; } = "";
        /// <summary>
        /// 用户账户
        /// </summary>
        public string UserAccountName { get; set; } = "";
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserInfoName { get; set; } = "";
        /// <summary>
        /// 登录AccessToken
        /// </summary>
        public string UserAccessToken { get; set; } = "";
    }
}
