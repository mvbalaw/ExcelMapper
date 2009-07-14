using System.IO;

using NUnit.Framework;

using RunTimeCodeGenerator.Logging;

namespace RunTimeCodeGenerator.Tests.Logging
{
    public class FileListenerTests
    {
        [TestFixture]
        public class When_asked_FileListener_to_log_a_message
        {
            [SetUp]
            public void SetUp()
            {
                File.Delete(TestData.LogFile);
            }

            [Test]
            public void Should_write_the_message_in_to_a_file()
            {
                const string message = "Application failed to launch";
                const string expectedMessage = "Error: " + message;
                FileListener fileListener = new FileListener(TestData.LogFile);
                fileListener.Writeline(MessageType.Error, message);
                Assert.IsTrue(File.Exists(TestData.LogFile));
                File.ReadAllText(TestData.LogFile).Equals(expectedMessage);
            }
        }
    }
}