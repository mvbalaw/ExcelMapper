using System;
using System.Collections.Generic;
using System.Linq;
using ExcelMapper.Configuration;
using ExcelMapper.Repository;
using ExcelMapper.Repository.Connection;
using ExcelMapper.Tests.DTO;
using NUnit.Framework;
using Rhino.Mocks;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Tests.Repository
{
    public class OleDbDataProviderTests
    {
        public class OleDbDataProviderTestsBase
        {
            private IConnectionString _connectionString;
            protected IDataProvider _dataProvider;
            protected IFileConfiguration _fileConfiguration;

            protected string _workSheetName;
            protected string _xlsFile;
            protected string _xlsxFile;


            [SetUp]
            public void SetUp()
            {
                _connectionString = MockRepository.GenerateMock<IConnectionString>();
                _fileConfiguration = MockRepository.GenerateMock<IFileConfiguration>();
                _dataProvider = new OleDbDataProvider(new ConnectionBuilder(_connectionString), _fileConfiguration);

                _xlsxFile = TestData.UsersXlsx;
                _xlsFile = TestData.UsersXls;
                _workSheetName = "User";

                _connectionString.Expect(x => x.Get(_xlsxFile)).Return(TestData.UsersXlsxConnectionString).Repeat.Any();
                _connectionString.Expect(x => x.Get(_xlsFile)).Return(TestData.UsersXlsConnectionString).Repeat.Any();
            }
        }

        [TestFixture]
        public class When_asked_for_the_list_of_worksheets_in_the_given_Excel_File : OleDbDataProviderTestsBase
        {
            [Test]
            public void Should_return_a_list_of_WorkSheet_names()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                List<string> workSheets = _dataProvider.GetTableNames().ToList();
                Assert.IsTrue(workSheets.Count == 3);
                Assert.IsTrue(workSheets.Exists(x => x.Equals(String.Format("{0}$", _workSheetName))));
            }
        }

        [TestFixture]
        public class When_asked_to_get_a_list_of_DTOs_for_the_given_type_from_Excel : OleDbDataProviderTestsBase
        {
            [Test]
            public void Should_map_the_excel_columns_to_the_given_DTO_type()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                User expectedUser = TestData.GetUsers(_xlsxFile, _workSheetName).FirstOrDefault();
                User actualUser = _dataProvider.Get<User>(_workSheetName).FirstOrDefault();

                Assert.IsNotNull(actualUser);

                Assert.AreEqual(expectedUser.Id, actualUser.Id);
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.DateOfBirth, actualUser.DateOfBirth);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            }

            [Test]
            public void Should_return_DTO_objects_for_each_row_in_the_Xls_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsFile);
                List<User> expectedUsers = TestData.GetUsers(_xlsFile, _workSheetName).ToList();
                List<User> actualUsers = _dataProvider.Get<User>(_workSheetName).ToList();
                Assert.IsNotNull(actualUsers);
                Assert.AreEqual(expectedUsers.Count, actualUsers.Count);
            }

            [Test]
            public void Should_return_DTO_objects_for_each_row_in_the_Xlsx_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                List<User> expectedUsers = TestData.GetUsers(_xlsxFile, _workSheetName).ToList();
                List<User> actualUsers = _dataProvider.Get<User>(_workSheetName).ToList();
                Assert.IsNotNull(actualUsers);
                Assert.AreEqual(expectedUsers.Count, actualUsers.Count);
            }
        }


        [TestFixture]
        public class When_asked_to_create_a_new_table : OleDbDataProviderTestsBase
        {
            [Test]
            public void Should_create_a_new_WorkSheet_in_the_given_file()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile).Repeat.Any();
                _dataProvider.CreateTable<Demo>();
                IEnumerable<string> tableNames = _dataProvider.GetTableNames();
                Assert.IsTrue(tableNames.Any(x => x == typeof (Demo).Name));
            }

            public class Demo
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public DateTime StartDate { get; set; }
                public decimal StartValue { get; set; }
            }
        }

        [TestFixture]
        public class When_asked_to_get_the_column_headers_for_a_given_WorkSheet : OleDbDataProviderTestsBase
        {
            [Test]
            public void Should_return_all_the_column_headers_with_type_in_the_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                List<Property> columns = _dataProvider.GetColumns(String.Format("{0}$", _workSheetName)).ToList();
                Assert.IsFalse(columns.Any(x => x.Name == null));
                Assert.IsFalse(columns.Any(x => x.Type == null));
            }

            [Test]
            public void Should_return_all_the_column_in_the_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                List<Property> columns = _dataProvider.GetColumns(String.Format("{0}$", _workSheetName)).ToList();
                Assert.IsNotNull(columns);
                Assert.IsTrue(columns.Count == 4);
            }
            
            [Test]
            public void Should_return_no_properties_if_the_Xls_worksheet_is_empty()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsFile);
                List<Property> columns = _dataProvider.GetColumns(String.Format("{0}$", "Sheet2")).ToList();
                Assert.IsEmpty(columns);
                Assert.IsTrue(columns.Count == 0);
            }
            
            [Test]
            public void Should_return_no_properties_if_the_Xlsx_worksheet_is_empty()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                List<Property> columns = _dataProvider.GetColumns(String.Format("{0}$", "Sheet2")).ToList();
                Assert.IsEmpty(columns);
                Assert.IsTrue(columns.Count == 0);
            }
        }

        [TestFixture]
        public class When_asked_to_insert_a_list_of_DTOs_into_an_Excel : OleDbDataProviderTestsBase
        {
            [Test]
            public void Should_insert_the_values_in_to_the_Xls_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsFile);
                var users = new List<User>
                                {
                                    new User
                                        {
                                            Id = 5,
                                            FirstName = "John",
                                            LastName = "Doe",
                                            DateOfBirth = Convert.ToDateTime("1/1/2008")
                                        }
                                };
                _dataProvider.Put(users);
            }

            [Test]
            public void Should_insert_the_values_in_to_the_Xlsx_worksheet()
            {
                _fileConfiguration.Expect(x => x.FileName).Return(_xlsxFile);
                var users = new List<User>
                                {
                                    new User
                                        {
                                            Id = 5,
                                            FirstName = "John",
                                            LastName = "Doe",
                                            DateOfBirth = Convert.ToDateTime("1/1/2008")
                                        }
                                };
                _dataProvider.Put(users);
            }
        }
    }
}