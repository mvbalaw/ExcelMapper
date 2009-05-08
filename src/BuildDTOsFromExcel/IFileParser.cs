using System.Collections.Generic;

namespace BuildDTOsFromExcel
{
    public interface IFileParser
    {
        List<string> Parse(List<string> files);
    }
}