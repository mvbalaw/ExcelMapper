using ExcelMapper.Repository;

using StructureMap.Configuration.DSL;

namespace ExcelMapper.Configuration
{
    public class ExcelMapperRegistry : Registry
    {
        public ExcelMapperRegistry()
        {
			For<IDataProvider>()
				.Use<OleDbDataProvider>();
            For<IRepository>()
                .Use<ExcelRepository>();
            For<IFileConfiguration>()
                .Singleton()
                .Use<FileConfiguration>();
            Scan(s =>
                     {
                         s.AssemblyContainingType<ExcelMapperRegistry>();
                         s.WithDefaultConventions();
                     });
        }
    }
}