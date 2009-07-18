using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ExcelMapper.Configuration;
using ExcelMapper.Repository;

using RunTimeCodeGenerator.AssemblyGeneration;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper
{
    public class ExcelToDTOMapper : IExcelToDTOMapper
    {
        private readonly IAssemblyGenerator _assemblyGenerator;
        private readonly IFileConfiguration _fileConfiguration;
        private readonly IClassGenerator _classGenerator;
        private readonly IRepository _excelRepository;

        public ExcelToDTOMapper(IRepository excelRepository, IClassGenerator classGenerator, IAssemblyGenerator assemblyGenerator, IFileConfiguration fileConfiguration)
        {
            _excelRepository = excelRepository;
            _classGenerator = classGenerator;
            _assemblyGenerator = assemblyGenerator;
            _fileConfiguration = fileConfiguration;
        }

        public bool Run(string assemblyName, List<string> files)
        {
            List<ClassAttributes> classAttributesList = new List<ClassAttributes>();

            foreach (string file in files)
            {
                _fileConfiguration.FileName = file;
                foreach (string workSheet in _excelRepository.GetWorkSheetNames())
                {
                    ClassAttributes classAttributes = _excelRepository.GetDTOClassAttributes(workSheet);
                    classAttributes.Namespace = String.Format("{0}.{1}", assemblyName, Path.GetFileNameWithoutExtension(file));

                    if (classAttributes.Properties.Count == 0)
                    {
                        continue;
                    }
                    _classGenerator.Create(classAttributes);
                    classAttributesList.Add(classAttributes);
                }
            }
            return ((classAttributesList.Count > 0)
                    && _assemblyGenerator.Compile(classAttributesList.Select(x => x.FullName).ToArray(), new AssemblyAttributes(assemblyName)));
        }
    }
}