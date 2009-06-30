using System.Data;

using ExcelMapper.Extensions;

using NUnit.Framework;

using Rhino.Mocks;

namespace ExcelMapper.Tests.Extensions
{
    public class DataReaderExtensionTests
    {
        [TestFixture]
        public class When_asked_to_get_a_value_from_the_datareader
        {
            private IDataReader _dataReader;

            [SetUp]
            public void SetUp()
            {
                _dataReader = MockRepository.GenerateMock<IDataReader>();
            }

            [Test]
            public void Should_give_the_value()
            {
                const string value = "Test";
                _dataReader.Expect(x => x.IsDBNull(0)).Return(false);
                _dataReader.Expect(x => x.GetValue(0)).Return(value);
                Assert.AreEqual(value, _dataReader.GetValue<string>(0));
            }

            [Test]
            public void Should_give_the_default_value_if_the_value_is_null()
            {
                _dataReader.Expect(x => x.IsDBNull(0)).Return(false);
                Assert.IsNull(_dataReader.GetValue<string>(0));
            }
        }
    }
}