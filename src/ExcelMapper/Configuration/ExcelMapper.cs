using Microsoft.Practices.ServiceLocation;

namespace ExcelMapper.Configuration
{
    public class ExcelMapper
    {
        public static void SetUp()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator());
            BootStrapper.Reset();
        }
    }
}