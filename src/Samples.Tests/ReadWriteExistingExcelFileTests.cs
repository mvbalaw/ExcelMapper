using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelMapper.Configuration;
using ExcelMapper.Repository;
using ExcelToDTOMapper.DTO.Users;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace Samples.Tests
{
    [TestFixture]
    public class ReadWriteExistingExcelFileTests
    {
        private IRepository _repository;
        private const string UserFile = "Excel\\Users.xlsx";
        private const string DemoWorkSheetFile = "Excel\\DemoWorkSheet.xlsx";

        [SetUp]
        public void BeforeEachTest()
        {
            File.Delete(DemoWorkSheetFile);
            ExcelMapper.Configuration.ExcelMapper.SetUp();
            _repository = ServiceLocator.Current.GetInstance<IRepository>();
        }

        public class DemoWorkSheet
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public decimal StartValue { get; set; }
        }

        [Test]
        public void ReadFromExcel()
        {
            ServiceLocator.Current.GetInstance<IFileConfiguration>().FileName = UserFile;
            foreach (User user in _repository.Get<User>("User"))
            {
                Console.WriteLine("Hello {0} {1}", user.FirstName, user.LastName);
            }
        }

        [Test]
        public void WriteToExcel()
        {
            ServiceLocator.Current.GetInstance<IFileConfiguration>().FileName = DemoWorkSheetFile;
            var expectedDemoWorkSheet = new DemoWorkSheet
                                   {
                                       Id = 8,
                                       Name = "John Doe",
                                       StartDate = Convert.ToDateTime("2/2/1990"),
                                       StartValue = 10.0m,
                                   };
            var demoWorkSheets = new List<DemoWorkSheet>
                            {
                                expectedDemoWorkSheet
                            };

            _repository.Save(demoWorkSheets);

            var enumerable = _repository.Get<DemoWorkSheet>(typeof(DemoWorkSheet).Name);
            DemoWorkSheet actualDemoWorkSheet = enumerable.Where(x => x.Id == expectedDemoWorkSheet.Id).First();
            Assert.IsNotNull(actualDemoWorkSheet);
            Assert.AreEqual(expectedDemoWorkSheet.Name, actualDemoWorkSheet.Name);
            Assert.AreEqual(expectedDemoWorkSheet.StartDate, actualDemoWorkSheet.StartDate);
            Assert.AreEqual(expectedDemoWorkSheet.StartValue, actualDemoWorkSheet.StartValue);
        }

        [Test]
        public void WriteToExistingExcel()
        {
            ServiceLocator.Current.GetInstance<IFileConfiguration>().FileName = UserFile;
            var expectedUser = new User
                                   {
                                       Id = 8,
                                       LastName = "Doe",
                                       FirstName = "John",
                                       DateOfBirth = Convert.ToDateTime("2/2/1990")
                                   };
            var users = new List<User>
                            {
                                expectedUser
                            };
            _repository.Save(users);

            User actualUser = _repository.Get<User>("User").Where(x => x.Id == expectedUser.Id).First();
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName);
            Assert.AreEqual(expectedUser.DateOfBirth, actualUser.DateOfBirth);
        }
    }
}