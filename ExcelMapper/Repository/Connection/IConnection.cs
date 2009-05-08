using System.Data.OleDb;

namespace ExcelMapper.Repository.Connection
{
    public interface IConnection
    {
        OleDbConnection GetConnection(string dataSource);
    }
}