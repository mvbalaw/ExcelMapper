using System.Collections.Generic;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public interface IDataProvider
    {
        void CreateTable<T>();
        IEnumerable<string> GetTableNames();
        IEnumerable<Property> GetColumns(string workSheet);
        IEnumerable<T> Get<T>(string workSheet);
        void Put<T>(IEnumerable<T> values);
    }
}