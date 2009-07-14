using System;
using System.Collections.Generic;

namespace RunTimeCodeGenerator.Logging
{
    public class Logger : ILogger
    {
        private readonly List<IListener> _listeners;

        public Logger(string sourceName)
        {
            SourceName = sourceName;

            _listeners = new List<IListener>
                {
                    new ConsoleListener()
                };
        }

        public List<IListener> Listeners
        {
            get { return _listeners; }
        }

        public string SourceName { get; private set; }

        public void LogInformation(string format, params object[] args)
        {
            Log(SourceName, MessageType.Information, format, args);
        }

        public void LogWarning(string format, params object[] args)
        {
            Log(SourceName, MessageType.Warning, format, args);
        }

        public void LogError(string format, params object[] args)
        {
            Log(SourceName, MessageType.Error, format, args);
        }

        private void Log(string sourceName, MessageType messageType, string format, object[] args)
        {
            foreach (IListener listener in _listeners)
            {
                listener.Writeline(messageType, String.Format("[{0}] {1}", sourceName, String.Format(format, args)));
            }
        }
    }
}