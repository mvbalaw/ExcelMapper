using System;
using System.IO;
using System.Reflection;

using BuildDTOsFromExcel.FileService;

using ExcelMapper;
using ExcelMapper.Configuration;

using Microsoft.Practices.ServiceLocation;

using NUnit.Framework;

namespace BuildDTOsFromExcel.Tests
{
    public class EngineTests
    {
        [TestFixture]
        public class When_given_a_list_of_excel_files
        {
            private IEngine _engine;

            [SetUp]
            public void SetUp()
            {
                ExcelMapper.Configuration.ExcelMapper.SetUp();
                _engine = new Engine(new FileParser(new FileSystemService()), ServiceLocator.Current.GetInstance<IExcelToDTOMapper>());
            }

            [Test]
            public void Should_create_class_files_for_each_tab_and_write_a_success_message_if_excelmapper_run_is_success()
            {
                string[] args = new[] { "TestDirectory\\Users.xlsx", "TestDirectory\\Roles.xlsx" };

                Assert.AreEqual(DefaultSettings.SuccessMessage, _engine.Run(args));
            }

            [Test]
            public void Should_create_classes_for_each_of_the_tabs_in_the_assembly()
            {
                Assembly assembly = Assembly.LoadFile(Path.GetFullPath("ExcelToDTOMapper.DTO.dll"));
                Type[] types = assembly.GetTypes();
                Assert.IsTrue(types.Length == 2);
                Assert.AreEqual("User", types[0].Name);
                Assert.AreEqual("Role", types[1].Name);
            }

            [Test]
            public void Should_write_an_error_message_if_the_excelmapper_fails()
            {
                string[] args = new[] { "TestDirectory\\User.xlsx" };

                Assert.AreEqual(DefaultSettings.ErrorMessage, _engine.Run(args));
            }
        }
    }
}