using System.Collections.Generic;

using ExcelMapper.DTO;

namespace ExcelMapper.Service
{
    public interface IAssemblyGenerator
    {
        bool Compile(List<ClassProperties> classPropertiesList, AssemblyProperties assemblyProperties, string logFile);
    }
}