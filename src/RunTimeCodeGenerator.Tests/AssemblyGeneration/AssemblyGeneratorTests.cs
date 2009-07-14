using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using RunTimeCodeGenerator.AssemblyGeneration;
using RunTimeCodeGenerator.ClassGeneration;

namespace RunTimeCodeGenerator.Tests.AssemblyGeneration
{
    public class AssemblyGeneratorTests
    {
        [TestFixture]
        public class When_given_a_list_of_class_files
        {
            private IClassGenerator _classGenerator;
            private IAssemblyGenerator _assemblyGenerator;
            private ClassAttributes _classAttributes;
            private AssemblyAttributes _assemblyAttributes;
            private string[] _classNames;

            [TestFixtureSetUp]
            public void TestFixtureSetUp()
            {
                File.Delete(TestData.LogFile);
            }

            [SetUp]
            public void SetUp()
            {
                BootStrapper.Reset();

                const string nameSpace = "Assembly.User";

                _classAttributes = new ClassAttributes("User")
                    {
                        Namespace = nameSpace,
                        Properties = new List<Property>
                            {
                                new Property("System.Double", "Id"),
                                new Property("System.String", "Name")
                            }
                    };
                _classNames = new[] { _classAttributes.FullName };

                _assemblyAttributes = new AssemblyAttributes(nameSpace);
                _classGenerator = new ClassFileGenerator();
                _assemblyGenerator = new AssemblyGenerator();
            }

            [TearDown]
            public void TearDown()
            {
                File.Delete(_classAttributes.FullName);
                File.Delete(_assemblyAttributes.FullName);
            }

            [Test]
            public void Should_generate_an_assembly()
            {
                _classGenerator.Create(_classAttributes);
                Assert.IsTrue(_assemblyGenerator.Compile(_classNames, _assemblyAttributes));
                Assert.IsTrue(File.Exists(_assemblyAttributes.FullName));
            }

            [Test]
            public void Should_generate_an_errorLog_if_it_cannot_compile_the_assembly()
            {
                _classAttributes.Properties.Add(new Property
                    {
                        Type = "Double"
                    });
                _classGenerator.Create(_classAttributes);

                Assert.IsFalse(_assemblyGenerator.Compile(_classNames, _assemblyAttributes));
                Assert.IsTrue(File.Exists(TestData.LogFile));
            }
        }
    }
}