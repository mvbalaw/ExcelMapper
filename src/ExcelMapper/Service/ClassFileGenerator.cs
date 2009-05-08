using System;
using System.IO;

using ExcelMapper.DTO;
using ExcelMapper.Service.FileService;

namespace ExcelMapper.Service
{
    public class ClassFileGenerator : IClassGenerator
    {
        private readonly IFileWriter _fileWriter;

        public ClassFileGenerator(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        public void Create(ClassProperties classProperties)
        {
            _fileWriter.Create(GetClassFile(classProperties.Name), FileMode.Create, FileAccess.Write);
            _fileWriter.WriteLine(String.Format("namespace {0}", classProperties.NameSpace));
            _fileWriter.WriteLine("{");
            _fileWriter.WriteLine(String.Format("\t public class {0}", classProperties.Name));
            _fileWriter.WriteLine("\t {");
            for (int i = 0; i < classProperties.Property.Count; i++)
            {
                _fileWriter.WriteLine(String.Format("\t\t public {0} {1} {2} get; set; {3}", classProperties.PropertyType[i], classProperties.Property[i], "{", "}"));
            }
            _fileWriter.WriteLine("\t}");
            _fileWriter.Write("}");
            _fileWriter.Close();
        }

        private static string GetClassFile(string className)
        {
            return String.Format("{0}.cs", className);
        }
    }
}