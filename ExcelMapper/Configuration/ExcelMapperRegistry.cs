using ExcelMapper.Repository;
using ExcelMapper.Service;

using StructureMap.Configuration.DSL;

namespace ExcelMapper.Configuration
{
    public class ExcelMapperRegistry : Registry
    {
        public ExcelMapperRegistry()
        {
            ForRequestedType<IClassGenerator>()
                .TheDefaultIsConcreteType<ClassFileGenerator>();
            ForRequestedType<IRepository>()
                .TheDefaultIsConcreteType<ExcelRepository>();
        }
    }
}