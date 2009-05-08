using System;
using System.Collections.Generic;
using System.IO;

using ExcelMapper.Service.FileService;

namespace BuildDTOsFromExcel
{
    public class FileParser : IFileParser
    {
        private readonly IFileSystemService _fileSystemService;
        private const string XlsSearchPattern = "*.xls";
        private const string XlsxSearchPattern = "*.xlsx";

        public FileParser(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        public List<string> Parse(List<string> files)
        {
            List<string> filesWithSearchPattern = files.FindAll(x => x.Contains(XlsSearchPattern) || x.Contains(XlsxSearchPattern));
            List<string> completeListOfFiles = files.FindAll(x => !x.Contains(XlsSearchPattern) && !x.Contains(XlsxSearchPattern));
            foreach (string file in filesWithSearchPattern)
            {
                completeListOfFiles.AddRange(GetFiles(file));
            }
            return completeListOfFiles;
        }

        private IEnumerable<string> GetFiles(string file)
        {
            string directory = Path.GetDirectoryName(file);
            string searchPattern = Path.GetFileName(file);
            if (String.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }
            return _fileSystemService.GetFiles(directory, searchPattern);
        }
    }
}