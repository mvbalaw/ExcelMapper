using System.Collections.Generic;

namespace ExcelMapper
{
    public interface IExcelToDTOMapper
    {
        bool Run(string assemblyName, List<string> files);
    }
}