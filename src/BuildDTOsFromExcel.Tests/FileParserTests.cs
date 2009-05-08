using System.Collections.Generic;
using System.IO;

using ExcelMapper.Service.FileService;

using NUnit.Framework;

using Rhino.Mocks;

namespace BuildDTOsFromExcel.Tests
{
    public class FileParserTests
    {
        [TestFixture]
        public class When_given_a_list_of_files
        {
            private IFileSystemService _fileSystemService;
            protected IFileParser _fileParser;
            protected List<string> _files;

            [SetUp]
            public void SetUp()
            {
                _fileSystemService = MockRepository.GenerateMock<IFileSystemService>();
                _fileParser = new FileParser(_fileSystemService);

                _files = new List<string>
                    {
                        "file1",
                        "file2"
                    };
            }

            [Test]
            public void Should_return_the_list_of_files()
            {
                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 2);
                Assert.AreEqual("file1", parsedFiles[0]);
                Assert.AreEqual("file2", parsedFiles[1]);
            }
        }

        [TestFixture]
        public class When_given_a_list_of_files_with_a_searchpattern
        {
            private IFileSystemService _fileSystemService;
            private IFileParser _fileParser;
            private List<string> _files;
            private List<string> _xlsxFilesinCurrentWorkingDirectory;
            private List<string> _xlsFilesinCurrentWorkingDirectory;
            private const string XlsSearchPattern = "*.xls";
            private const string XlsxSearchPattern = "*.xlsx";

            private const string DirectoryXlsSearchPattern = @"C:\Test\*.xls";
            private const string DirectoryXlsxSearchPattern = @"C:\Test\*.xlsx";

            [SetUp]
            public void SetUp()
            {
                _fileSystemService = MockRepository.GenerateMock<IFileSystemService>();
                _fileParser = new FileParser(_fileSystemService);

                _files = new List<string>
                    {
                        "file1",
                        "file2"
                    };

                _xlsxFilesinCurrentWorkingDirectory = new List<string>
                    {
                        "Users1.xlsx",
                        "Users2.xlsx"
                    };

                _xlsFilesinCurrentWorkingDirectory = new List<string>
                    {
                        "Users1.xls",
                        "Users2.xls"
                    };
            }

            [Test]
            public void Should_remove_the_searchpattern_and_return_all_the_xlsx_files_recursively_in_the_current_directory_for_the_xlsx_searchpattern()
            {
                _files.Add(XlsxSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(Directory.GetCurrentDirectory(), XlsxSearchPattern)).Return(_xlsxFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 4);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xlsx")));
            }

            [Test]
            public void Should_remove_the_searchpattern_and_return_all_the_xlsx_files_recursively_in_the_given_directory_for_the_xlsx_searchpattern()
            {
                _files.Add(DirectoryXlsxSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(null, null)).IgnoreArguments().Return(_xlsxFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 4);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xlsx")));
            }

            [Test]
            public void Should_remove_the_searchpattern_return_all_the_xls_files_recursively_in_the_current_directory_for_the_xls_searchpattern()
            {
                _files.Add(XlsSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(Directory.GetCurrentDirectory(), XlsSearchPattern)).Return(_xlsFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 4);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xls")));
            }

            [Test]
            public void Should_remove_the_searchpattern_return_all_the_xls_files_recursively_in_the_given_directory_for_the_xls_searchpattern()
            {
                _files.Add(DirectoryXlsSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(null, null)).IgnoreArguments().Return(_xlsFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 4);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xls")));
            }

            [Test]
            public void Should_remove_the_searchpattern_return_all_the_xls_and_xlsx_files_recursively_in_the_current_directory_if_both_xls_and_xlsx_searchpatterns_are_given()
            {
                _files.Add(XlsSearchPattern);
                _files.Add(XlsxSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(Directory.GetCurrentDirectory(), XlsSearchPattern)).Return(_xlsFilesinCurrentWorkingDirectory);
                _fileSystemService.Expect(x => x.GetFiles(Directory.GetCurrentDirectory(), XlsxSearchPattern)).Return(_xlsxFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 6);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xls")));
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users2.xlsx")));
            }

            [Test]
            public void Should_remove_the_searchpattern_return_all_the_xls_and_xlsx_files_recursively_in_the_given_directory_if_both_xls_and_xlsx_searchpatterns_are_given()
            {
                _files.Add(DirectoryXlsSearchPattern);
                _files.Add(DirectoryXlsxSearchPattern);

                _fileSystemService.Expect(x => x.GetFiles(null, null)).IgnoreArguments().Return(_xlsFilesinCurrentWorkingDirectory);
                _fileSystemService.Expect(x => x.GetFiles(null, null)).IgnoreArguments().Return(_xlsxFilesinCurrentWorkingDirectory);

                List<string> parsedFiles = _fileParser.Parse(_files);
                Assert.IsTrue(parsedFiles.Count == 6);
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users1.xls")));
                Assert.IsTrue(parsedFiles.Exists(x => x.Equals("Users2.xlsx")));
            }
        }
    }
}