using System.Collections.Generic;
using System.IO;

using ExcelMapper.DTO;
using ExcelMapper.Service;
using ExcelMapper.Service.FileService;

using NUnit.Framework;

namespace ExcelMapper.Tests.Service
{
    public class AssemblyGeneratorTests
    {
        [TestFixture]
        public class When_given_a_list_of_class_files
        {
            private IFileSystemService _fileSystemService;
            private IClassGenerator _classGenerator;
            private IAssemblyGenerator _assemblyGenerator;
            private ClassProperties _classProperties;
            private List<ClassProperties> _classPropertiesList;
            private AssemblyProperties _assemblyProperties;
            private const string LogFile = "TestLog";

            [SetUp]
            public void SetUp()
            {
                const string nameSpace = "Assembly.User";
                _classProperties = new ClassProperties("User")
                    {
                        NameSpace = nameSpace,
                        Property = new List<string>
                            {
                                "Id",
                                "Name"
                            },
                        PropertyType = new List<string>
                            {
                                "System.Double",
                                "System.String"
                            }
                    };

                _classPropertiesList = new List<ClassProperties>
                    {
                        _classProperties
                    };

                _assemblyProperties = new AssemblyProperties(nameSpace);
                _fileSystemService = new FileSystemService();
                _classGenerator = new ClassFileGenerator(new FileWriter());
                _assemblyGenerator = new AssemblyGenerator(new FileWriter());
            }

            [TearDown]
            public void TearDown()
            {
                _fileSystemService.Delete(_classProperties.FullName);
                _fileSystemService.Delete(LogFile);
                _fileSystemService.Delete(_assemblyProperties.FullName);
            }

            [Test]
            public void Should_generate_an_assembly()
            {
                _classGenerator.Create(_classProperties);
                Assert.IsTrue(_assemblyGenerator.Compile(_classPropertiesList, _assemblyProperties, LogFile));
                Assert.IsTrue(File.Exists(_assemblyProperties.FullName));
            }

            [Test]
            public void Should_generate_an_errorLog_if_it_cannot_compile_the_assembly()
            {
                _classProperties.PropertyType[0] = "Double";
                _classGenerator.Create(_classProperties);

                Assert.IsFalse(_assemblyGenerator.Compile(_classPropertiesList, _assemblyProperties, LogFile));
                Assert.IsTrue(File.Exists(LogFile));
            }
        }
    }
}