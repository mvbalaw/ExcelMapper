using System;

using StructureMap;

namespace RunTimeCodeGenerator.Logging
{
    public class Log
    {
        public static ILogger For(Type type)
        {
            return ObjectFactory.GetInstance<ILogFactory>()
                .Create(type);
        }
    }
}