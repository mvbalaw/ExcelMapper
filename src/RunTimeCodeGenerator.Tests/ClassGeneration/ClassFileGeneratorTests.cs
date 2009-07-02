using System;
using System.IO;

using NUnit.Framework;

using RunTimeCodeGenerator.ClassGeneration;

namespace RunTimeCodeGenerator.Tests.ClassGeneration
{
    public class ClassFileGeneratorTests
    {
        [TestFixture]
        public class When_asked_to_generate_a_class_file_with_only_properties
        {
            private IClassGenerator _classGenerator;
            private ClassAttributes _classAttributes;

            [SetUp]
            public void SetUp()
            {
                _classAttributes = new ClassAttributes("TestClass");
                _classAttributes.AddUsingNamespaces("System");
                _classAttributes.Properties.Add(new Property("Double", "TestItem1"));
                _classAttributes.Properties.Add(new Property("string", "TestItem2"));

                _classGenerator = new ClassFileGenerator();
            }

            [TearDown]
            public void TearDown()
            {
                File.Delete(_classAttributes.FullName);
            }

            [Test]
            public void Should_create_a_class_file()
            {
                _classGenerator.Create(_classAttributes);

                string fileName = String.Format("{0}.cs", _classAttributes.Name);
                Assert.IsTrue(File.Exists(fileName));
                Assert.IsTrue(new FileComparer().Compare(fileName, TestData.ClassWithProperties));
            }
        }

        [TestFixture]
        public class When_asked_to_generate_a_class_file
        {
            private IClassGenerator _classGenerator;
            private ClassAttributes _classAttributes;

            [SetUp]
            public void SetUp()
            {
                _classAttributes = new ClassAttributes("TestClass");
                _classAttributes.AddUsingNamespaces("System");
                _classAttributes.Namespace = "TestNamespace";
                Method method = new Method
                    {
                        Name = "TestMethod",
                        ReturnType = "string",
                        AccessLevel = AccessLevel.Public
                    };
                method.Body.Add("string variable = \"Test\";");
                method.Body.Add("Console.WriteLine(\"Hello\");");
                method.Body.Add("return variable;");
                _classAttributes.Methods.Add(method);

                _classGenerator = new ClassFileGenerator();
            }

            [TearDown]
            public void TearDown()
            {
                File.Delete(_classAttributes.FullName);
            }

            [Test]
            public void Should_create_a_class_file()
            {
                _classGenerator.Create(_classAttributes);

                string fileName = String.Format("{0}.cs", _classAttributes.Name);
                Assert.IsTrue(File.Exists(fileName));
                Assert.IsTrue(new FileComparer().Compare(fileName, TestData.ClassWithMethods));
            }
        }

        internal class FileComparer
        {
            public bool Compare(string file1, string file2)
            {
                using (FileStream fileStream1 = File.OpenRead(file1))
                {
                    using (FileStream fileStream2 = File.OpenRead(file2))
                    {
                        if (fileStream1.Length != fileStream2.Length)
                        {
                            return false;
                        }
                        while (fileStream1.ReadByte() != fileStream2.ReadByte())
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
        }
    }
}