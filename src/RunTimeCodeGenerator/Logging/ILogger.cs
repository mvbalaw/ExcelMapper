using System.Collections.Generic;

namespace RunTimeCodeGenerator.Logging
{
    public interface ILogger
    {
        List<IListener> Listeners { get; }

        string SourceName { get; }

        void LogInformation(string format, params object[] args);

        void LogWarning(string format, params object[] args);

        void LogError(string format, params object[] args);
    }
}