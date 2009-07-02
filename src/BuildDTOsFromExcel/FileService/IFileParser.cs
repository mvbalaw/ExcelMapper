using System.Collections.Generic;

namespace BuildDTOsFromExcel.FileService
{
    public interface IFileParser
    {
        List<string> Parse(List<string> files);
    }
}