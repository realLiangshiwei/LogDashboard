using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LogDashboard.NlogConfigParse
{
    public class FileNLogConfigParse : ILogConfigParse
    {

        public ILogConfigOptions Parse(string nLogConfig)
        {
            var config = XDocument.Load(Path.Combine(Directory.GetCurrentDirectory(), nLogConfig));

            var target = config.Root.Element("{http://www.nlog-project.org/schemas/NLog.xsd}targets").Elements().FirstOrDefault();

            if (target == null)
            {
                throw new ArgumentException("Not found target");
            }

            var logConfig = new FileNlogConfigOptions
            {
                LayoutDelimiter = "|"
            };

            return logConfig;
        }
    }
}
