namespace LogDashboard.NlogConfigParse
{
    public interface ILogConfigParse
    {
        ILogConfigOptions Parse(string nLogConfig);
    }
}
