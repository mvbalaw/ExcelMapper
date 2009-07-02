using System;
using System.Collections.Generic;
using System.IO;

using ExcelMapper.Configuration;
using ExcelMapper.Repository;

using Microsoft.Practices.ServiceLocation;

using NUnit.Framework;

using Rhino.Mocks;

using RunTimeCodeGenerator.AssemblyGeneration;
using RunTimeCodeGenerator.ClassGeneration;

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
            private ClassAttributes _classAttributes1;
            private ClassAttributes _classAttributes2;

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
                _classAttributes1 = new ClassAttributes("Test1");
                _classAttributes2 = new ClassAttributes("Test2");
            }

            [Test]
            public void Should_create_one_dto_for_each_of_the_worksheet()
            {
                AddProperties();

                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassAttributes("", "")).IgnoreArguments().Return(_classAttributes1);
                _excelRepository.Expect(x => x.GetClassAttributes("", "")).IgnoreArguments().Return(_classAttributes2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(true);

                Assert.IsTrue(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }

            [Test]
            public void Should_not_create_dto_if_there_are_no_properties()
            {
                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassAttributes("", "")).IgnoreArguments().Return(_classAttributes1);
                _excelRepository.Expect(x => x.GetClassAttributes("", "")).IgnoreArguments().Return(_classAttributes2);
                _classGenerator.Expect(x => x.Create(_classAttributes1)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(true);

                Assert.IsFalse(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }

            [Test]
            public void Should_not_create_an_assembly_if_the_compile_fails()
            {
                AddProperties();

                _excelRepository.Expect(x => x.GetWorkSheetNames("")).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetClassAttributes("", _workSheetNames[0])).IgnoreArguments().Return(_classAttributes1);
                _excelRepository.Expect(x => x.GetClassAttributes("", _workSheetNames[0])).IgnoreArguments().Return(_classAttributes2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(false);

                Assert.IsFalse(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }

            private void AddProperties()
            {
                _classAttributes1.AddProperty(new Property("Double", "Id"));
                _classAttributes2.AddProperty(new Property("Double", "Id"));
            }
        }

        [TestFixture]
        [Explicit]
        public class SystemTest
        {
            private IExcelToDTOMapper _excelToDtoMapper;
            private string _assemblyName;

            [SetUp]
            public void SetUp()
            {
                ExcelMapperServiceLocator.SetUp();
                _excelToDtoMapper = ServiceLocator.Current.GetInstance<IExcelToDTOMapper>();
                _assemblyName = String.Format("{0}.dll", TestData.AssemblyName);
            }

            [TearDown]
            public void TearDown()
            {
                File.Delete(_assemblyName);

                FileInfo[] files = GetCurrentDirectoryCSFiles();

                foreach (var file in files)
                {
                    File.Delete(file.Name);
                }
            }

            private static FileInfo[] GetCurrentDirectoryCSFiles()
            {
                return new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.cs");
            }

            [Test]
            public void Should_create_an_assembly_of_dto_corresponding_to_worksheets_in_the_excel()
            {
                List<string> excelFiles = new List<string>
                    {
                        TestData.UsersXlsx
                    };
                Assert.IsTrue(_excelToDtoMapper.Run(TestData.AssemblyName, excelFiles));

                Assert.IsTrue(File.Exists(_assemblyName));
                Assert.IsTrue(GetCurrentDirectoryCSFiles().Length > 0);
            }
        }
    }
}