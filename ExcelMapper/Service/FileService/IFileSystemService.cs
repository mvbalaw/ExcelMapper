using System.Collections.Generic;

namespace ExcelMapper.Service.FileService
{
    public interface IFileSystemService
    {
        IEnumerable<string> GetFiles(string directory, string searchPattern);
        string GetFilePath(string fileName, string directory);
        void Delete(string file);
    }
}