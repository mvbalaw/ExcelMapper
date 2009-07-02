using System;

namespace RunTimeCodeGenerator.ClassGeneration
{
    public class ClassFileGenerator : IClassGenerator
    {
        public void Create(ClassAttributes classAttributes)
        {
            using (IClassFileWriter classFileWriter = new ClassFileWriter(classAttributes.FullName))
            {
                foreach (var usingNamespace in classAttributes.UsingNamespaces)
                {
                    AddUsing(classFileWriter, usingNamespace);
                }

                classFileWriter.Write();

                StartNamespace(classFileWriter, classAttributes.Namespace);
                {
                    StartClass(classFileWriter, classAttributes.Name);
                    {
                        foreach (var property in classAttributes.Properties)
                        {
                            AddProperty(classFileWriter, property);
                        }

                        foreach (var method in classAttributes.Methods)
                        {
                            StartMethod(classFileWriter, method);
                            {
                                AddMethod(classFileWriter, method);
                            }
                            classFileWriter.CloseBlock();
                        }
                    }
                    classFileWriter.CloseBlock();
                }
                CloseNamespace(classFileWriter, classAttributes.Namespace);
            }
        }

        private static void AddUsing(IClassFileWriter classFileWriter, string usingNamespace)
        {
            classFileWriter.Write(String.Format("using {0};", usingNamespace));
        }

        private static void StartNamespace(IClassFileWriter classFileWriter, string classNamespace)
        {
            if (String.IsNullOrEmpty(classNamespace))
            {
                return;
            }
            classFileWriter.Write(String.Format("namespace {0}", classNamespace));
            classFileWriter.StartBlock();
        }

        private static void CloseNamespace(IClassFileWriter classFileWriter, string classNamespace)
        {
            if (String.IsNullOrEmpty(classNamespace))
            {
                return;
            }
            classFileWriter.CloseBlock();
        }

        private static void StartClass(IClassFileWriter classFileWriter, string className)
        {
            classFileWriter.Write(String.Format("{0} class {1}", AccessLevel.Public.Value, className));
            classFileWriter.StartBlock();
        }

        private static void StartMethod(IClassFileWriter classFileWriter, Method method)
        {
            classFileWriter.Write(String.Format("{0} {1} {2}()", method.AccessLevel.Value, method.ReturnType, method.Name));
            classFileWriter.StartBlock();
        }

        private static void AddProperty(IClassFileWriter classFileWriter, Property property)
        {
            classFileWriter.Write(String.Format("{0} {1} {2} {{ get; set; }}", AccessLevel.Public.Value, property.Type, property.Name));
        }

        private static void AddMethod(IClassFileWriter classFileWriter, Method method)
        {
            foreach (var line in method.Body)
            {
                string formattedLine = line;
                bool endBlock = line.StartsWith("}");
                bool startBlock = line.EndsWith("{");

                if (endBlock)
                {
                    if (formattedLine.Length > 1)
                    {
                        formattedLine = formattedLine.Substring(1).Trim();
                    }
                    classFileWriter.CloseBlock();
                }
                if (startBlock)
                {
                    if (formattedLine.Length > 1)
                    {
                        formattedLine = formattedLine.Substring(0, formattedLine.Length - 1);
                    }
                }

                if (line.Length > 1)
                {
                    classFileWriter.Write(formattedLine);
                }
                if (startBlock && formattedLine != "{")
                {
                    classFileWriter.StartBlock();
                }
            }
        }
    }
}