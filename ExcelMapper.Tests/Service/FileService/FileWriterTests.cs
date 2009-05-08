using System.IO;

using ExcelMapper.Service.FileService;

using NUnit.Framework;

namespace ExcelMapper.Tests.Service.FileService
{
    public class FileWriterTests
    {
        public class FileWriterTestsBase
        {
            protected IFileWriter _fileWriter;
            protected const string FileName = "Test";
            protected const string Text = "Testing...";

            [SetUp]
            public void SetUp()
            {
                _fileWriter = new FileWriter();
                _fileWriter.Create(FileName, FileMode.Create, FileAccess.Write);
            }

            [TearDown]
            public void TearDown()
            {
                File.Delete(FileName);
            }
        }

        [TestFixture]
        public class When_asked_to_create_a_file : FileWriterTestsBase
        {
            [Test]
            public void Should_create_a_file_with_the_right_access_rights_and_mode()
            {
                _fileWriter.Close();
                Assert.IsTrue(File.Exists(FileName));
            }
        }

        [TestFixture]
        public class When_given_a_string : FileWriterTestsBase
        {
            [Test]
            public void Should_write_the_string_in_the_file()
            {
                _fileWriter.WriteLine(Text);
                _fileWriter.Close();

                Assert.IsTrue(File.ReadAllText(FileName).Contains(Text));
            }
        }

        [TestFixture]
        public class When_asked_to_close_the_file : FileWriterTestsBase
        {
            [Test]
            public void Should_dispose_the_handle_to_the_file()
            {
                _fileWriter.WriteLine(Text);
                _fileWriter.Close();

                try
                {
                    File.ReadAllText(FileName);
                }
                catch (IOException)
                {
                    Assert.Fail("File is not closed properly");
                }
            }
        }
    }
}