using System.Collections.Generic;
using System.Linq;

using ExcelMapper.Repository;

using RunTimeCodeGenerator.AssemblyGeneration;
using RunTimeCodeGenerator.ClassGeneration;

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
            List<ClassAttributes> classAttributesList = new List<ClassAttributes>();

            foreach (string file in files)
            {
                foreach (string workSheet in _excelRepository.GetWorkSheetNames(file))
                {
                    ClassAttributes classAttributes = _excelRepository.GetClassAttributes(file, workSheet);
                    classAttributes.Namespace = assemblyName;

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