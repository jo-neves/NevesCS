namespace NevesCS.Abstractions.Services
{
    public interface INevesCsLogger : IDisposable
    {
        public void LogTrace(string content);

        public void LogDebug(string content);

        public void LogInformation(string content);

        public void LogWarning(string content);

        public void LogError(string content);

        public void LogCritical(string content);
    }
}
