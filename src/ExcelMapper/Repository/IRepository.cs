using System.Collections.Generic;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public interface IRepository
    {
        IEnumerable<string> GetWorkSheetNames();
        ClassAttributes GetDTOClassAttributes(string workSheet);
        IEnumerable<T> Get<T>(string workSheet);
        void Save<T>(IEnumerable<T> values);
    }
}