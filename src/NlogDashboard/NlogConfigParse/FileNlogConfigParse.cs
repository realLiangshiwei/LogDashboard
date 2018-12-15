using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NLogDashboard.NlogConfigParse
{
    public class FileNLogConfigParse : INLogConfigParse
    {

        private const string LayoutRegex = "{[a-z]+";

        public FileNLogConfigParse()
        {
        }

        public NlogConfigParseModel Parse(string nLogConfig)
        {
            var config = XDocument.Load(Path.Combine(Directory.GetCurrentDirectory(), nLogConfig));

            var target = config.Root.Element("targets").Elements().FirstOrDefault();

            if (target == null)
            {
                throw new ArgumentException("Not found target");
            }

            var fileName = target.Attribute("fileName")?.Value;

            if (fileName == null)
            {
                throw new ArgumentException("target FileName property is empty");
            }

            var layout = target.Attribute("layout")?.Value;

            if (layout == null)
            {
                throw new ArgumentException("target layout property is empty");
            }

            var matches = Regex.Matches(layout, LayoutRegex);

            var nlogconfig = new FileNlogConfigParseModel()
            {
                FileName = fileName,
                LayoutDelimiter = "|"
            };

            foreach (Match match in matches)
            {
                nlogconfig.Fields.Add(match.Value.Trim('{'));
            }

            return nlogconfig;
        }
    }
}
