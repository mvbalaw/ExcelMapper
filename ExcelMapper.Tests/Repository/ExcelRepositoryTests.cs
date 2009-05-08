using System;
using System.Collections.Generic;
using System.Linq;

using ExcelMapper.DTO;
using ExcelMapper.Repository;
using ExcelMapper.Repository.Connection;
using ExcelMapper.Tests.DTO;

using NUnit.Framework;

using Rhino.Mocks;

namespace ExcelMapper.Tests.Repository
{
    public class ExcelRepositoryTests
    {
        public class ExcelRepositoryTestsBase
        {
            private IConnectionString _connectionString;
            protected IRepository _excelRepository;

            protected string _xlsxFile;
            protected string _xlsFile;
            protected string _workSheetName;

            [SetUp]
            public void SetUp()
            {
                _connectionString = MockRepository.GenerateMock<IConnectionString>();
                _excelRepository = new ExcelRepository(new global::ExcelMapper.Repository.Connection.Connection(_connectionString));

                _xlsxFile = TestData.UsersXlsx;
                _xlsFile = TestData.UsersXls;
                _workSheetName = "User";

                _connectionString.Expect(x => x.Get(_xlsxFile)).Return(TestData.UsersXlsxConnectionString);
                _connectionString.Expect(x => x.Get(_xlsFile)).Return(TestData.UsersXlsConnectionString);
            }
        }

        [TestFixture]
        public class When_given_an_Excel_File : ExcelRepositoryTestsBase
        {
            [Test]
            public void Should_give_the_list_of_WorkSheets_in_the_excel()
            {
                List<string> workSheets = _excelRepository.GetWorkSheetNames(_xlsxFile).ToList();
                Assert.IsTrue(workSheets.Count == 3);
                Assert.IsTrue(workSheets.Exists(x => x.Equals(String.Format("{0}$", _workSheetName))));
            }
        }

        [TestFixture]
        public class When_given_a_WorkSheet : ExcelRepositoryTestsBase
        {
            [Test]
            public void Should_return_Class_Properties_object_that_includes_all_the_properties_in_the_class()
            {
                ClassProperties properties = _excelRepository.GetClassProperties(_xlsxFile, String.Format("{0}$", _workSheetName));
                Assert.IsNotNull(properties);
                Assert.AreEqual(_workSheetName, properties.Name);
                Assert.IsTrue(properties.Property.Count == 4);
                Assert.IsTrue(properties.PropertyType.Count == 4);
            }
        }

        [TestFixture]
        public class When_given_a_excel_file_name_and_worksheet_name : ExcelRepositoryTestsBase
        {
            [Test]
            public void Should_map_the_excel_columns_to_the_given_dto_object()
            {
                User expectedUser = TestData.GetUsers(_xlsxFile, _workSheetName).FirstOrDefault();
                User actualUser = _excelRepository.Get<User>(_xlsxFile, _workSheetName).FirstOrDefault();

                Assert.IsNotNull(actualUser);

                Assert.AreEqual(expectedUser.Id, actualUser.Id);
                Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
                Assert.AreEqual(expectedUser.DateOfBirth, actualUser.DateOfBirth);
                Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            }

            [Test]
            public void Should_return_dto_objects_for_each_row_in_the_Xlsx_worksheet()
            {
                List<User> expectedCourts = TestData.GetUsers(_xlsxFile, _workSheetName).ToList();
                List<User> actualCourts = _excelRepository.Get<User>(_xlsxFile, _workSheetName).ToList();
                Assert.IsNotNull(actualCourts);
                Assert.AreEqual(expectedCourts.Count, actualCourts.Count);
            }

            [Test]
            public void Should_return_dto_objects_for_each_row_in_the_Xls_worksheet()
            {
                List<User> expectedSuits = TestData.GetUsers(_xlsFile, _workSheetName).ToList();
                List<User> actualSuits = _excelRepository.Get<User>(_xlsFile, _workSheetName).ToList();
                Assert.IsNotNull(actualSuits);
                Assert.AreEqual(expectedSuits.Count, actualSuits.Count);
            }
        }
    }
}