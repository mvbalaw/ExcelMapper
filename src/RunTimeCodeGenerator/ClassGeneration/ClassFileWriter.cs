using System.IO;

namespace RunTimeCodeGenerator.ClassGeneration
{
    public class ClassFileWriter : IClassFileWriter
    {
        private readonly FileStream _fileStream;
        private readonly StreamWriter _streamWriter;
        private int _indent;
        private bool _isDisposed;

        public ClassFileWriter(string className)
        {
            _fileStream = new FileStream(className, FileMode.Create, FileAccess.Write);
            _streamWriter = new StreamWriter(_fileStream);
            _indent = 0;
        }

        public void StartBlock()
        {
            Write("{");
            _indent++;
        }

        public void CloseBlock()
        {
            _indent--;
            Write("}");
        }

        public void Write()
        {
            _streamWriter.WriteLine();
        }

        public void Write(string value)
        {
            _streamWriter.WriteLine("".PadLeft(_indent, '\t') + value);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _streamWriter.Close();
            _fileStream.Close();
            _isDisposed = true;
        }
    }
}