using System;
using System.IO;

using ExcelMapper.DTO;
using ExcelMapper.Service;
using ExcelMapper.Service.FileService;

using NUnit.Framework;

namespace ExcelMapper.Tests.Service
{
    public class ClassFileGeneratorTests
    {
        [TestFixture]
        public class When_asked_to_generate_a_class_file
        {
            private IClassGenerator _classGenerator;
            private ClassProperties _classProperties;

            [SetUp]
            public void SetUp()
            {
                _classGenerator = new ClassFileGenerator(new FileWriter());
                _classProperties = new ClassProperties("TestClass");
                _classProperties.PropertyType.Add("Double");
                _classProperties.Property.Add("TestItem");
            }

            [TearDown]
            public void TearDown()
            {
                new FileSystemService().Delete(_classProperties.FullName);
            }

            [Test]
            public void Should_create_a_class_file()
            {
                _classGenerator.Create(_classProperties);

                Assert.IsTrue(File.Exists(String.Format("{0}.cs", _classProperties.Name)));
            }
        }
    }
}