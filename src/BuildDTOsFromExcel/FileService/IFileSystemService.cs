using System.Collections.Generic;

namespace BuildDTOsFromExcel.FileService
{
    public interface IFileSystemService
    {
        IEnumerable<string> GetFiles(string directory, string searchPattern);
        string GetFilePath(string fileName, string directory);
    }
}