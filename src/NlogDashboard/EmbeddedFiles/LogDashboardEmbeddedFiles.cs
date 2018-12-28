using System.IO;
using System.Reflection;
using LogDashboard.Route;

namespace LogDashboard.EmbeddedFiles
{
    public class LogDashboardEmbeddedFiles
    {
        public static string IncludeEmbeddedFile(string path)
        {
            var stream = Assembly.GetAssembly(typeof(LogDashboardRoute)).GetManifestResourceStream($"{LogDashboardConsts.Root}.{path.Substring(1)}");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
