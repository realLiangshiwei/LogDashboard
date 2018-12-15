using System.IO;
using System.Reflection;
using NLogDashboard.Route;

namespace NLogDashboard
{
    public class NLogDashboardEmbeddedFiles
    {
        public static string IncludeEmbeddedFile(string path)
        {
            var stream = Assembly.GetAssembly(typeof(NLogDashboardRoute)).GetManifestResourceStream("NLogDashboard." + path.Substring(1));
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
