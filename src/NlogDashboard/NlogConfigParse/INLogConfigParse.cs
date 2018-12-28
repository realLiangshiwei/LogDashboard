namespace NLogDashboard.NlogConfigParse
{
    public interface INLogConfigParse
    {
        ILogConfigOptions Parse(string nLogConfig);
    }
}
