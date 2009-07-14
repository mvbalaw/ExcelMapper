using System.Collections.Generic;

using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public interface IRepository
    {
        IEnumerable<string> GetWorkSheetNames();
        ClassAttributes GetClassAttributes(string workSheet);
        IEnumerable<T> Get<T>(string workSheet);
    }
}