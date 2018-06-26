using System.IO;
using System.Reflection;
using NlogDashboard.Route;

namespace NlogDashboard
{
    public class NlogDashboardEmbeddedFiles
    {
        public static string IncludeEmbeddedFile(string path)
        {
            var stream = Assembly.GetAssembly(typeof(NlogDashboardRoute)).GetManifestResourceStream("NlogDashboard." + path.Substring(1));
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
