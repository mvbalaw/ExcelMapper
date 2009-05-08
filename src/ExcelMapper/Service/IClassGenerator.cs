using ExcelMapper.DTO;

namespace ExcelMapper.Service
{
    public interface IClassGenerator
    {
        void Create(ClassProperties classProperties);
    }
}