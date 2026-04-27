using Serilog;
using System;

namespace ProyectoTicketTurno.Infrastructure.Logging
{
    public static class LoggerManager
    {
        private static ILogger _logger;

        static LoggerManager()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public static void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            if (ex != null)
                _logger.Error(ex, message);
            else
                _logger.Error(message);
        }

        public static void LogDebug(string message)
        {
            _logger.Debug(message);
        }
    }
}
