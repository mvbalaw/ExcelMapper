using System.Data.OleDb;

namespace ExcelMapper.Repository.Connection
{
    public interface IConnectionBuilder
    {
        OleDbConnection GetConnection(string dataSource);
    }
}