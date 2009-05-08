using System.IO;

namespace ExcelMapper.Service.FileService
{
    public class FileWriter : IFileWriter
    {
        private FileStream _fileStream;
        private StreamWriter _streamWriter;
        private bool _isDisposed;

        public void Create(string fileName, FileMode fileMode, FileAccess fileAccess)
        {
            _isDisposed = false;
            _fileStream = new FileStream(fileName, fileMode, fileAccess);
            _streamWriter = new StreamWriter(_fileStream);
        }

        public void Write(string value)
        {
            _streamWriter.Write(value);
        }

        public void WriteLine(string value)
        {
            _streamWriter.WriteLine(value);
        }

        public void Close()
        {
            Dispose();
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