using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace ExcelMapper.Tests
{
    [TestFixture]
    public class SystemTest
    {
        private IExcelToDTOMapper _excelToDtoMapper;

        [SetUp]
        public void SetUp()
        {
            Configuration.ExcelMapper.SetUp();
            _excelToDtoMapper = ServiceLocator.Current.GetInstance<IExcelToDTOMapper>();
        }

        [TearDown]
        public void TearDown()
        {
            FileInfo[] files = GetCurrentDirectoryCSFiles();

            foreach (FileInfo file in files)
            {
                File.Delete(file.Name);
            }

            if (Directory.Exists(TestData.LogsDirectory))
            {
                Directory.Delete(TestData.LogsDirectory, true);
            }
        }

        private static FileInfo[] GetCurrentDirectoryCSFiles()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.cs");
        }

        [Test]
        public void Should_create_an_assembly_of_dto_corresponding_to_worksheets_in_the_excel()
        {
            var excelFiles = new List<string>
                                 {
                                     TestData.UsersXlsx
                                 };

            const string assembly = "UserTest1";
            string assemblyNameWithExtension = String.Format("{0}.dll", assembly);

            Assert.IsTrue(_excelToDtoMapper.Run(assembly, excelFiles));
            Assert.IsTrue(File.Exists(assemblyNameWithExtension));
            Assert.IsTrue(GetCurrentDirectoryCSFiles().Length > 0);
        }

        [Test]
        public void
            Should_create_an_assembly_of_dto_corresponding_to_worksheets_in_the_excel_using_filename_as_the_namespace
            ()
        {
            var excelFiles = new List<string>
                                 {
                                     TestData.UsersXlsx
                                 };

            const string assemblyName = "UserTest2";
            string assemblyNameWithExtension = String.Format("{0}.dll", assemblyName);

            bool assemblyCompiled = _excelToDtoMapper.Run(assemblyName, excelFiles);

            if (!assemblyCompiled)
            {
                Assert.Fail("Assembly failed to compile");
            }
            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(assemblyNameWithExtension));
            Type[] types = assembly.GetTypes();
            Assert.IsTrue(types.Any(x => x.Namespace == String.Format("{0}.UsersXlsx", assemblyName)));
        }

        [Test]
        public void Should_create_an_assembly_of_dto_with_identical_classes_in_two_different_namespaces()
        {
            var excelFiles = new List<string>
                                 {
                                     TestData.UsersXls,
                                     TestData.UsersXlsx
                                 };
            const string assemblyName = "UserTest3";
            string assemblyNameWithExtension = String.Format("{0}.dll", assemblyName);

            bool assemblyCompiled = _excelToDtoMapper.Run(assemblyName, excelFiles);

            if (!assemblyCompiled)
            {
                Assert.Fail("Assembly failed to compile");
            }
            Assembly assembly = Assembly.LoadFile(Path.GetFullPath(assemblyNameWithExtension));
            Type[] types = assembly.GetTypes();

            Assert.IsTrue(types.Length == 2);
            Assert.IsTrue(types.Any(x => x.Namespace == String.Format("{0}.UsersXlsx", assemblyName)));
            Assert.IsTrue(types.Any(x => x.Namespace == String.Format("{0}.UsersXls", assemblyName)));
        }
    }
}