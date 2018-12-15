using System;
using NLogDashboard.NlogConfigParse;

namespace NlogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var parse = new FileNLogConfigParse();

            parse.Parse("NLog.config");


            var logger = NLog.LogManager.GetLogger("logfile");
            logger.Info("Hello World");
            try
            {
                throw new Exception("eee");
            }
            catch (Exception e)
            {
                logger.Error(e);
            }


            Console.WriteLine("Hello World!");
        }
    }
}
