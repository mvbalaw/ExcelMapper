using ExcelMapper.Repository;

using StructureMap.Configuration.DSL;

namespace ExcelMapper.Configuration
{
    public class ExcelMapperRegistry : Registry
    {
        public ExcelMapperRegistry()
        {
			ForRequestedType<IDataProvider>()
				.TheDefaultIsConcreteType<OleDbDataProvider>();
            ForRequestedType<IRepository>()
                .TheDefaultIsConcreteType<ExcelRepository>();
            ForRequestedType<IFileConfiguration>()
                .AsSingletons()
                .TheDefaultIsConcreteType<FileConfiguration>();
            Scan(s =>
                     {
                         s.AssemblyContainingType<ExcelMapperRegistry>();
                         s.WithDefaultConventions();
                     });
        }
    }
}