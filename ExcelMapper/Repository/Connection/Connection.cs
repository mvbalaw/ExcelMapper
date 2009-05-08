using System.Data.OleDb;

namespace ExcelMapper.Repository.Connection
{
    public class Connection : IConnection
    {
        private readonly IConnectionString _connectionString;

        public Connection(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public OleDbConnection GetConnection(string file)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString.Get(file));
            connection.Open();
            return connection;
        }
    }
}