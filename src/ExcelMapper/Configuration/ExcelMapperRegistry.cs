using ExcelMapper.Repository;

using StructureMap.Configuration.DSL;

namespace ExcelMapper.Configuration
{
    public class ExcelMapperRegistry : Registry
    {
        public ExcelMapperRegistry()
        {
            ForRequestedType<IRepository>()
                .TheDefaultIsConcreteType<ExcelRepository>();

            Scan(s =>
                     {
                         s.AssemblyContainingType<ExcelMapperRegistry>();
                         s.WithDefaultConventions();
                     });
        }
    }
}