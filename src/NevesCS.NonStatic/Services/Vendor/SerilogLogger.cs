using Microsoft.Extensions.Logging;
using NevesCS.Abstractions.Services;

using Serilog;

namespace NevesCS.NonStatic.Services.Vendor
{
    public class SerilogLogger : INevesCsLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger Logger;

        public SerilogLogger(Serilog.ILogger logger)
        {
            Logger = new LoggerFactory().AddSerilog(logger).CreateLogger<Serilog.ILogger>();
        }

        private IDisposable? Scope { get; set; }

        public void Dispose()
        {
            Scope?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            Scope = Logger.BeginScope(state);
            return Scope;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public void LogDebug(string content)
        {
            Logger.LogDebug(content);
        }

        public void LogError(string content)
        {
            Logger.LogError(content);
        }

        public void LogCritical(string content)
        {
            Logger.LogCritical(content);
        }

        public void LogInformation(string content)
        {
            Logger.LogInformation(content);
        }

        public void LogTrace(string content)
        {
            Logger.LogTrace(content);
        }

        public void LogWarning(string content)
        {
            Logger.LogWarning(content);
        }
    }
}
