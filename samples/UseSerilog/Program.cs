using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace UseSerilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File($"{AppContext.BaseDirectory}Log/.log", rollingInterval:RollingInterval.Day,outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} || {Level} || {SourceContext:l} || {Message} || {Exception} || {MachineName} || {EnvironmentUserName} || {ProcessId} || {ThreadId} || {ClientIp} || {UserAgent} || {Enterpriseid} || {RequestPath} || {RequestMethod} || {Referer} || {UserAccountId} || {UserInfoId} || {UserAccountName} || {UserInfoName}  || {UserAccessToken} || end {NewLine}")
                .CreateLogger();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();

    }
}
