using NUnit.Framework;

using RunTimeCodeGenerator.Logging;

namespace RunTimeCodeGenerator.Tests.Logging
{
    public class LogTests
    {
        public class Test
        {
        }

        [TestFixture]
        public class When_asked_for_Log
        {
            [SetUp]
            public void SetUp()
            {
                BootStrapper.Reset();
            }

            [Test]
            public void Should_return_Logger()
            {
                Assert.IsInstanceOfType(typeof(Logger), Log.For(typeof(Test)));
            }

            [Test]
            public void Should_return_logger_with_Console_and_FileListeners()
            {
                Assert.IsTrue(Log.For(typeof(Test)).Listeners.Exists(x => x.GetType() == typeof(ConsoleListener)));
                Assert.IsTrue(Log.For(typeof(Test)).Listeners.Exists(x => x.GetType() == typeof(FileListener)));
            }
        }
    }
}