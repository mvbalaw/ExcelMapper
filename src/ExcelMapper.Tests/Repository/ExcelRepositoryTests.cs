using System;
using System.Collections.Generic;
using System.Linq;
using ExcelMapper.Configuration;
using ExcelMapper.Repository;
using ExcelMapper.Tests.DTO;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace ExcelMapper.Tests.Repository
{
    public class ExcelRepositoryTests
    {
        [TestFixture]
        public class When_asked_to_retrieve_an_entity_from_an_Excel_file
        {
            private IDataProvider _dataProvider;
            private DateTime _fileModifiedDate;
            private IFileService _fileService;
            private IRepository _repository;
            private List<User> _users;

            [SetUp]
            public void SetUp()
            {
                _users = new List<User>
                             {
                                 new User
                                     {
                                         Id = 1,
                                         LastName = "LastName",
                                         FirstName = "FirstName"
                                     }
                             };
                _fileModifiedDate = Convert.ToDateTime("1/1/2009");

                var autoMocker = new RhinoAutoMocker<ExcelRepository>();
                _fileService = autoMocker.Get<IFileService>();
                _dataProvider = autoMocker.Get<IDataProvider>();
                _repository = autoMocker.ClassUnderTest;

                _fileService.Expect(x => x.GetLastModifiedDate("")).IgnoreArguments().Return(_fileModifiedDate).Repeat.
                    Any();
                _dataProvider.Expect(x => x.Get<User>("")).IgnoreArguments().Return(_users);
            }


            [Test]
            public void Should_get_the_values_from_cache_or_file()
            {
                List<User> result = _repository.Get<User>("").ToList();
                result = _repository.Get<User>("").ToList();
                Assert.AreEqual(_users.Count, result.Count);
            }
        }

        [TestFixture]
        public class When_asked_to_SaveOrUpdate_an_entity_into_an_excel_file
        {
            private const string Testfile = "TestFile";
            private IDataProvider _dataProvider;
            private IRepository _excelRepository;
            private IFileConfiguration _fileConfiguration;
            private IFileService _fileService;
            private List<User> _users;

            [SetUp]
            public void SetUp()
            {
                _users = new List<User>
                             {
                                 new User
                                     {
                                         Id = 1,
                                         LastName = "LastName",
                                         FirstName = "FirstName"
                                     }
                             };
                Convert.ToDateTime("1/1/2009");

                var autoMocker = new RhinoAutoMocker<ExcelRepository>();
                _fileService = autoMocker.Get<IFileService>();
                _dataProvider = autoMocker.Get<IDataProvider>();
                _excelRepository = autoMocker.ClassUnderTest;
                _fileConfiguration = autoMocker.Get<IFileConfiguration>();

                _fileConfiguration.Expect(x => x.FileName).Return(Testfile).Repeat.Any();
            }

            [Test]
            public void Should_create_an_Excel_file_if_it_exists()
            {
                _fileService.Expect(x => x.Exists(Testfile)).Return(true);
                _dataProvider.Expect(x => x.GetTableNames()).Return(new List<string>());

                _excelRepository.SaveOrUpdate(_users);
                _fileService.AssertWasNotCalled(x => x.Create(Testfile));
            }

            [Test]
            public void Should_create_the_WorkSheet_if_it_doesnot_exist()
            {
                _fileService.Expect(x => x.Exists(Testfile)).Return(true);
                _dataProvider.Expect(x => x.GetTableNames()).Return(new List<string>());

                _excelRepository.SaveOrUpdate(_users);
                _dataProvider.AssertWasCalled(x => x.CreateTable<User>());
            }

            [Test]
            public void Should_not_create_the_WorkSheet_if_it_exists()
            {
                _fileService.Expect(x => x.Exists(Testfile)).Return(true);
                _dataProvider.Expect(x => x.GetTableNames()).Return(new List<string> {typeof (User).Name});

                _excelRepository.SaveOrUpdate(_users);
                _dataProvider.AssertWasNotCalled(x => x.CreateTable<User>());
            }

            [Test]
            public void Should_save_the_values_in_to_excel()
            {
                _fileService.Expect(x => x.Exists(Testfile)).Return(true);
                _dataProvider.Expect(x => x.GetTableNames()).Return(new List<string> {typeof (User).Name});

                _excelRepository.SaveOrUpdate(_users);
                _dataProvider.AssertWasCalled(x => x.Put(_users));
            }
        }
    }
}