using System;

namespace RunTimeCodeGenerator.Logging
{
    public class ConsoleListener : IListener
    {
        public void Writeline(MessageType messageType, string message)
        {
            Console.WriteLine("{0}: {1}", messageType.Value, message);
        }
    }
}