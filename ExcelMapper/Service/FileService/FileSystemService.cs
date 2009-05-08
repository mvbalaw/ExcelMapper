using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelMapper.Service.FileService
{
    public class FileSystemService : IFileSystemService
    {
        public IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            if (String.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles(searchPattern, SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                yield return file.FullName;
            }
        }

        public string GetFilePath(string fileName, string directory)
        {
            return String.IsNullOrEmpty(directory) ? fileName : Path.Combine(directory, fileName);
        }

        public void Delete(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}