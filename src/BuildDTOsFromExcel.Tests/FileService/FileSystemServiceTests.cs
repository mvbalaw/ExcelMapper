using System;
using System.Linq;

using BuildDTOsFromExcel.FileService;

using NUnit.Framework;

namespace BuildDTOsFromExcel.Tests.FileService
{
    public class FileSystemServiceTests
    {
        [TestFixture]
        public class When_given_a_directory
        {
            private IFileSystemService _fileSystemService;
            private const string Directory = "TestDirectory";
            private const string SearchPattern = "*.txt";

            [SetUp]
            public void SetUp()
            {
                _fileSystemService = new FileSystemService();
            }

            [Test]
            public void Should_return_files_in_current_working_directory_if_directory_name_is_null_or_empty()
            {
                Assert.IsTrue(_fileSystemService.GetFiles(null, SearchPattern).ToList().Count > 0);
            }
            
            [Test]
            public void Should_give_the_list_of_files_within_the_directory()
            {
                Assert.IsTrue(_fileSystemService.GetFiles(Directory, SearchPattern).ToList().Count > 0);
            }
        }

        [TestFixture]
        public class When_asked_for_filepath
        {
            private IFileSystemService _fileSystemService;
            private string _directoryName;
            private string _fileName;

            [SetUp]
            public void SetUp()
            {
                _fileSystemService = new FileSystemService();
                _directoryName = "directory";
                _fileName = "User.cs";
            }

            [Test]
            public void Should_give_the_filename_if_the_directory_is_null_or_empty()
            {
                Assert.AreEqual(_fileName, _fileSystemService.GetFilePath(_fileName, ""));
            }

            [Test]
            public void Should_return_the_directory_filename_combination_path()
            {
                Assert.AreEqual(String.Format("{0}\\{1}", _directoryName, _fileName), _fileSystemService.GetFilePath(_fileName, _directoryName));
            }
        }
    }
}