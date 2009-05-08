using System.Collections.Generic;

using ExcelMapper.DTO;

namespace ExcelMapper.Repository
{
    public interface IRepository
    {
        IEnumerable<string> GetWorkSheetNames(string fileName);
        ClassProperties GetClassProperties(string file, string workSheet);
        IEnumerable<T> Get<T>(string file, string workSheet);
    }
}