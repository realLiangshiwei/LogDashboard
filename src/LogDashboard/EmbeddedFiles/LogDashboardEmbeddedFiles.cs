using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using LogDashboard.Route;
#if NETFRAMEWORK
using Microsoft.Owin;
#endif

#if NETSTANDARD2_0
using Microsoft.AspNetCore.Http;
#endif


namespace LogDashboard.EmbeddedFiles
{
    public class LogDashboardEmbeddedFiles
    {
        static readonly Dictionary<string, string> ResponseType = new Dictionary<string, string>
        {
            { ".css","text/css"},
            { ".js","application/javascript"}
        };
#if NETSTANDARD2_0
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
#endif


#if NETFRAMEWORK
        public static string IncludeEmbeddedFile(IOwinContext context, string path)
        {
            context.Response.ContentType = ResponseType[Path.GetExtension(path)];
            var stream = Assembly.GetAssembly(typeof(LogDashboardRoute)).GetManifestResourceStream($"{LogDashboardConsts.Root}.{path.Substring(1)}");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
#endif

    }
}
