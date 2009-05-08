using StructureMap;

namespace ExcelMapper.Configuration
{
    public class BootStrapper
    {
        private static bool _initialized;

        public static void Initialize()
        {
            ObjectFactory.Initialize(x => x.Scan(s =>
                                                     {
                                                         s.TheCallingAssembly();
                                                         s.WithDefaultConventions();
                                                         s.LookForRegistries();
                                                     }));
        }

        public static void Reset()
        {
            if (_initialized)
            {
                ObjectFactory.ResetDefaults();
            }
            else
            {
                Initialize();
                _initialized = true;
            }
        }
    }
}