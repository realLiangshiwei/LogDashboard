using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using LogDashboard.Route;
using Microsoft.AspNetCore.Http;

namespace LogDashboard.EmbeddedFiles
{
    public class LogDashboardEmbeddedFiles
    {
        static Dictionary<string, string> ResponseType = new Dictionary<string, string>
        {
            { ".css","text/css"},
            { ".js","application/javascript"}
        };

        public static string IncludeEmbeddedFile(HttpContext context, string path)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.ContentType = ResponseType[Path.GetExtension(path)];
                }

                return Task.CompletedTask;
            });

            var stream = Assembly.GetAssembly(typeof(LogDashboardRoute)).GetManifestResourceStream($"{LogDashboardConsts.Root}.{path.Substring(1)}");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
