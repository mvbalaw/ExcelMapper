using ExcelMapper.DTO;

namespace ExcelMapper.Service
{
    public interface IAssemblyGenerator
    {
        bool Compile(string[] classNames, AssemblyProperties assemblyProperties, string logFile);
    }
}