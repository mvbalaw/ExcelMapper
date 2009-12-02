using System.Data.OleDb;

namespace ExcelMapper.Repository.Connection
{
    public class ConnectionBuilder : IConnectionBuilder
    {
        private readonly IConnectionString _connectionString;

        public ConnectionBuilder(IConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public OleDbConnection GetConnection(string file)
        {
            var connection = new OleDbConnection(_connectionString.Get(file));
            connection.Open();
            return connection;
        }
    }
}