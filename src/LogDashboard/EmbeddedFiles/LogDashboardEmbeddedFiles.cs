using System;
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
        static readonly Dictionary<string, string> ResponseType = new Dictionary<string, string>
        {
            { ".css","text/css"},
            { ".js","application/javascript"},
            {".woff2","font/woff2" },
            {".woff","font/woff" },
            {".ttf","application/octet-stream" },
            {".jpg","image/jpeg" },
        };

        private static readonly Assembly Assembly;

        static LogDashboardEmbeddedFiles()
        {
            Assembly = Assembly.GetExecutingAssembly();
        }

        public static async Task IncludeEmbeddedFile(HttpContext context, string path)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.ContentType = ResponseType[Path.GetExtension(path)];
                }

                return Task.CompletedTask;
            });

            try
            {
                using var inputStream = Assembly.GetManifestResourceStream($"{LogDashboardConsts.Root}.{path.Substring(1)}");
                if (inputStream == null)
                {
                    throw new ArgumentException($@"Resource with name {path.Substring(1)} not found in assembly {Assembly}.");
                }
                await inputStream.CopyToAsync(context.Response.Body).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await context.Response.WriteAsync($"{e.StackTrace}");
            }

        }
    }
}
