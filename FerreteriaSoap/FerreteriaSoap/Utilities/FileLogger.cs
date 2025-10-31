using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace FerreteriaSoap.Utilities
{
    public static class FileLogger
    {
        private static string GetLogPath()
        {
            var dir = HostingEnvironment.MapPath("~/App_Data/logs");
            if (dir == null) return null;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return Path.Combine(dir, "ferreteria.log");
        }

        public static void Error(string category, string message, Exception ex = null)
        {
            try
            {
                var path = GetLogPath();
                if (path == null) return;
                var sb = new StringBuilder();
                sb.Append(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append(" UTC | ERROR | ");
                sb.Append(category);
                sb.Append(" | ");
                sb.Append(message);
                if (ex != null)
                {
                    sb.Append(" | ");
                    sb.Append(ex.GetType().FullName);
                    sb.Append(": ");
                    sb.Append(ex.Message);
                    sb.Append(" | ");
                    sb.Append(ex.StackTrace);
                }
                sb.AppendLine();
                File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
            }
            catch
            {
                // no-op: evitar romper el flujo del servicio por errores de logging
            }
        }
    }
}
