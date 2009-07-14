using System.IO;

namespace RunTimeCodeGenerator.Logging
{
    public class FileListener : IListener
    {
        private readonly string _logFile;

        public FileListener(string logFile)
        {
            _logFile = logFile;
        }

        public void Writeline(MessageType messageType, string message)
        {
            using (var fileStream = new FileStream(_logFile, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write("{0}: {1}", messageType.Value, message);
            }
        }
    }
}