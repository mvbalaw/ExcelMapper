using System;

namespace RunTimeCodeGenerator.Logging
{
    public class LogFactory : ILogFactory
    {
        public ILogger Create(Type type)
        {
            // All types get both console and file logging
            Logger logger = new Logger(type.Name);
            logger.Listeners.Add(new FileListener(DefaultSettings.LogFile));
            return logger;
        }
    }
}