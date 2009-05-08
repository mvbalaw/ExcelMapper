using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ExcelMapper.Configuration;
using ExcelMapper.DTO;
using ExcelMapper.Repository;
using ExcelMapper.Service;
using ExcelMapper.Service.FileService;

using Microsoft.Practices.ServiceLocation;

using NUnit.Framework;

using Rhino.Mocks;

namespace ExcelMapper.Tests
{
    public class ExcelToDTOMapperTests
    {
        [TestFixture]
        public class When_asked_to_convert_excel_worksheets_to_DTO
        {
            private IRepository _excelRepository;
            private IClassGenerator _classGenerator;
            private IAssemblyGenerator _assemblyGenerator;
            private IExcelToDTOMapper _excelToDtoMapper;

            private List<string> _excelFiles;
            private List<string> _workSheetNames;
            private ClassProperties _classProperties1;
            private ClassProperties _classProperties2;

            [SetUp]
            public void SetUp()
            {
                _excelRepository = MockRepository.GenerateMock<IRepository>();
                _classGenerator = MockRepository.GenerateMock<IClassGenerator>();
                _assemblyGenerator = MockRepository.GenerateMock<IAssemblyGenerator>();
                _excelToDtoMapper = new ExcelToDTOMapper(_excelRepository, _classGenerator, _assemblyGenerator);

                _excelFiles = new List<string>
                    {
                        "TestExcel"
                    };
                _workSheetNames = new List<string>
                    {
                        "Test1$",
                        "Test2$"
                    };
                _classProperties1 = new ClassProperties("Test1");
                _classProperties2 = new ClassProperties("Test2");
            }

            [Test]
            public void Should_create_one_dto_for_each_of_the_worksheet()
            {
                AddProperties();
                AddPropertyTypes();

                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties1);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null, null)).IgnoreArguments().Return(true);

                Assert.IsTrue(_excelToDtoMapper.Run(DefaultSettings.Assembly, _excelFiles));
            }

            [Test]
            public void Should_not_create_dto_if_there_are_no_properties()
            {
                AddPropertyTypes();
                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties1);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties2);
                _classGenerator.Expect(x => x.Create(_classProperties1)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null, null)).IgnoreArguments().Return(true);

                Assert.IsFalse(_excelToDtoMapper.Run(DefaultSettings.Assembly, _excelFiles));
            }

            [Test]
            public void Should_not_create_dto_if_there_are_no_propertyTypes()
            {
                AddProperties();
                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties1);
                _excelRepository.Expect(x => x.GetClassProperties("", "")).IgnoreArguments().Return(_classProperties2);
                _classGenerator.Expect(x => x.Create(_classProperties1)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null, null)).IgnoreArguments().Return(true);

                Assert.IsFalse(_excelToDtoMapper.Run(DefaultSettings.Assembly, _excelFiles));
            }

            [Test]
            public void Should_not_create_an_assembly_if_the_compile_fails()
            {
                AddProperties();
                AddPropertyTypes();
                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassProperties("", _workSheetNames[0])).IgnoreArguments().Return(_classProperties1);
                _excelRepository.Expect(x => x.GetClassProperties("", _workSheetNames[0])).IgnoreArguments().Return(_classProperties2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null, null)).IgnoreArguments().Return(false);

                Assert.IsFalse(_excelToDtoMapper.Run(DefaultSettings.Assembly, _excelFiles));
            }

            private void AddProperties()
            {
                _classProperties1.AddProperty("Id");
                _classProperties2.AddProperty("Id");
            }

            private void AddPropertyTypes()
            {
                _classProperties1.AddPropertyType("Double");
                _classProperties2.AddPropertyType("Double");
            }
        }

        [TestFixture]
        [Explicit]
        public class SystemTest
        {
            private IExcelToDTOMapper _excelToDtoMapper;
            private IFileSystemService _fileSystemService;
            private string _assemblyName;

            [SetUp]
            public void SetUp()
            {
                ExcelMapperServiceLocator.SetUp();
                _fileSystemService = ServiceLocator.Current.GetInstance<IFileSystemService>();
                _excelToDtoMapper = ServiceLocator.Current.GetInstance<IExcelToDTOMapper>();
                _assemblyName = String.Format("{0}.dll", DefaultSettings.Assembly);
            }

            [TearDown]
            public void TearDown()
            {
                _fileSystemService.Delete(_assemblyName);
                _fileSystemService.Delete(DefaultSettings.LogFile);
                foreach (string file in _fileSystemService.GetFiles("", "*.cs"))
                {
                    _fileSystemService.Delete(file);
                }
            }

            [Test]
            public void Should_create_an_assembly_of_dto_corresponding_to_worksheets_in_the_excel()
            {
                List<string> excelFiles = new List<string>
                    {
                        TestData.UsersXlsx
                    };
                Assert.IsTrue(_excelToDtoMapper.Run(DefaultSettings.Assembly, excelFiles));

                Assert.IsTrue(File.Exists(_assemblyName));
                Assert.IsTrue(_fileSystemService.GetFiles("", "*.cs").ToList().Count() > 0);
            }
        }
    }
}