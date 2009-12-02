using System;
using System.IO;

namespace ExcelMapper.Repository
{
    public class FileService : IFileService
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void Create(string filePath)
        {
            File.Create(filePath);
        }

        public DateTime GetLastModifiedDate(string filePath)
        {
            return File.GetLastWriteTime(filePath);
        }
    }
}