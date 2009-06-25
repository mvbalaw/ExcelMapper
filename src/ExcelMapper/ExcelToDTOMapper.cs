using System.Collections.Generic;
using System.Linq;

using ExcelMapper.DTO;
using ExcelMapper.Repository;
using ExcelMapper.Service;

namespace ExcelMapper
{
    public class ExcelToDTOMapper : IExcelToDTOMapper
    {
        private readonly IAssemblyGenerator _assemblyGenerator;
        private readonly IClassGenerator _classGenerator;
        private readonly IRepository _excelRepository;

        public ExcelToDTOMapper(IRepository excelRepository, IClassGenerator classGenerator, IAssemblyGenerator assemblyGenerator)
        {
            _excelRepository = excelRepository;
            _classGenerator = classGenerator;
            _assemblyGenerator = assemblyGenerator;
        }

        public bool Run(string assemblyName, List<string> files)
        {
            List<ClassProperties> classPropertiesList = new List<ClassProperties>();

            foreach (string file in files)
            {
                foreach (string workSheet in _excelRepository.GetWorkSheetNames(file))
                {
                    ClassProperties classProperties = _excelRepository.GetClassProperties(file, workSheet);
                    classProperties.NameSpace = assemblyName;

                    if (classProperties.Property.Count == 0 || classProperties.PropertyType.Count == 0)
                    {
                        continue;
                    }
                    _classGenerator.Create(classProperties);
                    classPropertiesList.Add(classProperties);
                }
            }
            return ((classPropertiesList.Count > 0)
                    && _assemblyGenerator.Compile(classPropertiesList.Select(x => x.FullName).ToArray(), new AssemblyProperties(assemblyName), DefaultSettings.LogFile));
        }
    }
}