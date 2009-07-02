using System.Collections.Generic;

using NUnit.Framework;

namespace BuildDTOsFromExcel.Tests
{
    public class ExtensionTests
    {
        [TestFixture]
        public class When_asked_for_an_AssemblyName
        {
            [Test]
            public void Should_return_the_assembly_name_it_if_it_exists_in_the_array_of_arguments()
            {
                string[] args = new[] { "/Assembly:MyAssembly", "file1", "file2" };
                string assemblyName = args.GetAssemblyName();
                Assert.AreEqual("MyAssembly", assemblyName);
            }

            [Test]
            public void Should_return_the_default_assembly_name_it_if_it_exists_in_the_array_of_arguments()
            {
                string[] args = new[] { "file1", "file2" };
                string assemblyName = args.GetAssemblyName();
                Assert.AreEqual(DefaultSettings.AssemblyName, assemblyName);
            }
        }

        [TestFixture]
        public class When_asked_for_files
        {
            [Test]
            public void Should_return_the_file_names_that_are_in_the_array_of_arguments()
            {
                string[] args = new[] { "/Assembly:MyAssembly", "file1", "file2", "*.xlsx" };
                List<string> files = args.GetFiles();
                Assert.IsTrue(files.Count == 3);
                Assert.IsFalse(files.Exists(x => x.Equals("/Assembly:MyAssembly")));
            }
        }
    }
}