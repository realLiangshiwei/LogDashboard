namespace LogDashboard.Extensions
{
    public static class LogDashboardOptionsExtensions
    {
        /// <summary>
        /// 设置根目录
        /// </summary>
        /// <param name="options">LogDashboardOptions</param>
        /// <param name="rootPath">日志根目录哦</param>
        public static void SetRootPath(this LogDashboardOptions options,string rootPath)
        {
            options.RootPath=rootPath;
        }

        /// <summary>
        /// 设置分隔符
        /// </summary>
        /// <param name="options">LogDashboardOptions</param>
        /// <param name="startDelimiter">分隔符</param>
        /// <param name="endDelimiter">结束分隔符</param>
        public static void SetDelimiter(this LogDashboardOptions options,string startDelimiter="||",string endDelimiter="||end")
        {
            options.FileEndDelimiter=startDelimiter;
            options.FileFieldDelimiter=endDelimiter;
        }
    }
}