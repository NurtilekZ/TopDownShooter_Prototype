namespace _current.Services.Logging
{
    public interface ILoggingService
    {
        void LogMessage(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger);
        void LogWarning(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger);
        void LogError(string message, object sender = null, LoggingTag loggingTag = LoggingTag.Logger);
    }
}