using System;

using ExcelMapper.Repository.Connection;

using NUnit.Framework;

namespace ExcelMapper.Tests.Repository.Connection
{
    public class ConnectionStringTests
    {
        [TestFixture]
        public class When_given_an_excel_file
        {
            private IConnectionString _connectionString;

            [SetUp]
            public void SetUp()
            {
                _connectionString = new ConnectionString();
            }

            [Test]
            public void Should_return_the_connection_string_for_xlsx_file_extension()
            {
                Assert.AreEqual(TestData.UsersXlsxConnectionString, _connectionString.Get(TestData.UsersXlsx));
            }

            [Test]
            public void Should_return_the_connection_string_for_xls_file_extension()
            {
                Assert.AreEqual(TestData.UsersXlsConnectionString, _connectionString.Get(TestData.UsersXls));
            }

            [Test, ExpectedException(typeof(ArgumentException))]
            public void Should_throw_an_exception_for_an_unknown_file_extension()
            {
                Assert.AreEqual(TestData.UsersXlsConnectionString, _connectionString.Get("Users.xl"));
            }
        } 
    }
}