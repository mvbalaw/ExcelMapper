using System;

namespace RunTimeCodeGenerator.Logging
{
    public interface ILogFactory
    {
        ILogger Create(Type type);
    }
}