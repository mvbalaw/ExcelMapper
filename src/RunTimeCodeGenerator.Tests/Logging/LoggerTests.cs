using NUnit.Framework;

using RunTimeCodeGenerator.Logging;

namespace RunTimeCodeGenerator.Tests.Logging
{
    public class LoggerTests
    {
        [TestFixture]
        public class When_a_Logger_is_created
        {
            [Test]
            public void Should_include_consoleListener_by_default()
            {
                Assert.IsTrue(new Logger("Test").Listeners.Exists(x => x.GetType() == typeof(ConsoleListener)));
            }

            [Test]
            public void Should_set_the_SourceName()
            {
                const string sourceName = "Test";
                Assert.AreEqual(sourceName, new Logger(sourceName).SourceName);
            }
        }
    }
}