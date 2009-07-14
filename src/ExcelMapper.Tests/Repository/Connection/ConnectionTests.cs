using System.Data;
using System.Data.OleDb;

using ExcelMapper.Repository.Connection;

using NUnit.Framework;

using Rhino.Mocks;

namespace ExcelMapper.Tests.Repository.Connection
{
    public class ConnectionTests
    {
        [TestFixture]
        public class When_asked_for_a_connection
        {
            private string _file;
            private IConnectionString _connectionString;
            private IConnectionBuilder _connectionBuilder;

            [SetUp]
            public void SetUp()
            {
                _file = TestData.UsersXlsx;
                _connectionString = MockRepository.GenerateMock<IConnectionString>();
                _connectionBuilder = new global::ExcelMapper.Repository.Connection.ConnectionBuilder(_connectionString);
            }

            [Test]
            public void Should_open_a_new_OleDbConnection()
            {
                _connectionString.Expect(x => x.Get(_file)).Return(TestData.UsersXlsConnectionString);

                using (OleDbConnection connection = _connectionBuilder.GetConnection(_file))
                {
                    Assert.IsInstanceOfType(typeof(OleDbConnection), connection);
                    Assert.AreEqual(ConnectionState.Open, connection.State);
                }
            }
        }
    }
}