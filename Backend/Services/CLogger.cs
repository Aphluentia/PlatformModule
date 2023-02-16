using Microsoft.AspNetCore.Http;

namespace WebPlatform.Services
{
    public class CLogger : ICLogger
    {
        private readonly string logsFile;
        public CLogger(IConfiguration configuration)
        {
            logsFile = configuration.GetSection("Logging:LogFile").Get<string>();
        }

        void ICLogger.Error(string message, string? clientId)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"ERROR",-10}{clientId,-35}{message,-125}");
        }


        void ICLogger.Info(string message, string? clientId)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"INFO",-10}{clientId,-35}{message,-125}");
        }

        void ICLogger.System(string message, string? clientId)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"SYSTEM",-10}{message,-125}");
        }

        void ICLogger.Warn(string message, string? clientId)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"ERROR",-10}{clientId,-35}{message,-125}");
        }

        void ICLogger.Error(string message)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"ERROR",-10}{message,-125}");
        }


        void ICLogger.Info(string message)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"INFO",-10}{message,-125}");
        }

        void ICLogger.System(string message)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"SYSTEM",-10}{message,-125}");
        }

        void ICLogger.Warn(string message)
        {
            WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),-25}{"ERROR",-10}{message,-125}");
        }
        void WriteLine(string logString)
        {
            if (!System.IO.File.Exists(logsFile))
            {
                using (StreamWriter file = new(logsFile))
                {
                    file.WriteLineAsync(logString);
                }
            }
            else
            {
                using StreamWriter file = new(logsFile, append: true);
                file.WriteLineAsync(logString);
            }
        }
    }
}
