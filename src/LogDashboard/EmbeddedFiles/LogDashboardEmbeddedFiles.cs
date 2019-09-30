using System;
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
            { ".js","application/javascript"},
            {".woff2","font/woff2" },
            {".woff","font/woff" },
            {".ttf","application/octet-stream" },
        };

        private static Assembly _assembly;

        static LogDashboardEmbeddedFiles()
        {
            _assembly = Assembly.GetAssembly(typeof(LogDashboardRoute));
        }

#if NETSTANDARD2_0
        public static void IncludeEmbeddedFile(HttpContext context, string path)
        {

            context.Response.OnStarting(() =>
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.ContentType = ResponseType[Path.GetExtension(path)];
                }

                return Task.CompletedTask;
            });

            using (var inputStream = _assembly.GetManifestResourceStream($"{LogDashboardConsts.Root}.{path.Substring(1)}"))
            {
                if (inputStream == null)
                {
                    throw new ArgumentException($@"Resource with name {path.Substring(1)} not found in assembly {_assembly}.");
                }

                inputStream.CopyTo(context.Response.Body);
            }
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
