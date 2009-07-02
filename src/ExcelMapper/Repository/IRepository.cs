using System.Collections.Generic;

using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public interface IRepository
    {
        IEnumerable<string> GetWorkSheetNames(string fileName);
        ClassAttributes GetClassAttributes(string file, string workSheet);
        IEnumerable<T> Get<T>(string file, string workSheet);
    }
}