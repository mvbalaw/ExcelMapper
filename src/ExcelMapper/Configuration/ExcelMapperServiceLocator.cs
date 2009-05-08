using Microsoft.Practices.ServiceLocation;

namespace ExcelMapper.Configuration
{
    public class ExcelMapperServiceLocator
    {
        public static void SetUp()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator());
            BootStrapper.Reset();
        }
    }
}