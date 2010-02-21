using System.Collections.Generic;
using ExcelMapper.Configuration;
using ExcelMapper.Repository;
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
            private IAssemblyGenerator _assemblyGenerator;
            private ClassAttributes _classAttributes1;
            private ClassAttributes _classAttributes2;
            private IClassGenerator _classGenerator;
            private List<string> _excelFiles;
            private IRepository _excelRepository;
            private IExcelToDTOMapper _excelToDtoMapper;
            private IFileConfiguration _fileConfiguration;
            private List<string> _workSheetNames;

            [SetUp]
            public void SetUp()
            {
                _excelRepository = MockRepository.GenerateMock<IRepository>();
                _classGenerator = MockRepository.GenerateMock<IClassGenerator>();
                _assemblyGenerator = MockRepository.GenerateMock<IAssemblyGenerator>();
                _fileConfiguration = MockRepository.GenerateMock<IFileConfiguration>();
                _excelToDtoMapper = new ExcelToDTOMapper(_excelRepository, _classGenerator, _assemblyGenerator,
                                                         _fileConfiguration);

                _excelFiles = new List<string>
                                  {
                                      "TestExcel.xls"
                                  };
                _workSheetNames = new List<string>
                                      {
                                          "Test1$",
                                          "Test2$"
                                      };
                _classAttributes1 = new ClassAttributes("Test1");
                _classAttributes2 = new ClassAttributes("Test2");
            }

            private void AddProperties()
            {
                _classAttributes1.AddProperty(new Property("Double", "Id"));
                _classAttributes2.AddProperty(new Property("Double", "Id"));
            }

            [Test]
            public void Should_create_one_dto_for_each_of_the_worksheet()
            {
                AddProperties();

                _excelRepository.Expect(x => x.GetWorkSheetNames()).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetDTOClassAttributes("")).IgnoreArguments().Return(_classAttributes1);
                _excelRepository.Expect(x => x.GetDTOClassAttributes("")).IgnoreArguments().Return(_classAttributes2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(true);

                Assert.IsTrue(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }

            [Test]
            public void Should_not_create_an_assembly_if_the_compile_fails()
            {
                AddProperties();

                _excelRepository.Expect(x => x.GetWorkSheetNames()).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetDTOClassAttributes(_workSheetNames[0])).IgnoreArguments().Return(
                    _classAttributes1);
                _excelRepository.Expect(x => x.GetDTOClassAttributes(_workSheetNames[0])).IgnoreArguments().Return(
                    _classAttributes2);
                _classGenerator.Expect(x => x.Create(null)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(false);

                Assert.IsFalse(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }

            [Test]
            public void Should_not_create_dto_if_there_are_no_properties()
            {
                _excelRepository.Expect(x => x.GetWorkSheetNames()).IgnoreArguments().Return(_workSheetNames);
                _excelRepository.Expect(x => x.GetDTOClassAttributes("")).IgnoreArguments().Return(_classAttributes1);
                _excelRepository.Expect(x => x.GetDTOClassAttributes("")).IgnoreArguments().Return(_classAttributes2);
                _classGenerator.Expect(x => x.Create(_classAttributes1)).IgnoreArguments();
                _assemblyGenerator.Expect(x => x.Compile(null, null)).IgnoreArguments().Return(true);

                Assert.IsFalse(_excelToDtoMapper.Run(TestData.AssemblyName, _excelFiles));
            }
        }
    }

}