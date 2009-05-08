using System;
using System.IO;

namespace ExcelMapper.Service.FileService
{
    public interface IFileWriter : IDisposable
    {
        void Create(string fileName, FileMode fileMode, FileAccess fileAccess);
        void Write(string value);
        void WriteLine(string value);
        void Close();
    }
}