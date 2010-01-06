using System;

namespace ExcelMapper.Repository
{
    public interface IFileService
    {
        bool Exists(string filePath);
        void Create(string filePath);
        DateTime GetLastModifiedDate(string filePath);
    }
}