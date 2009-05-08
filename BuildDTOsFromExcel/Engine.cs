using System;
using System.Collections.Generic;

using ExcelMapper;
using ExcelMapper.Configuration;
using ExcelMapper.Service.FileService;

using Microsoft.Practices.ServiceLocation;

namespace BuildDTOsFromExcel
{
    public class Engine : IEngine
    {
        public string Run(string[] args)
        {
            ExcelMapperServiceLocator.SetUp();

            string assemblyName = args.GetAssemblyName();
            List<string> files = new FileParser(ServiceLocator.Current.GetInstance<IFileSystemService>()).Parse(args.GetFiles());
            return ServiceLocator.Current.GetInstance<IExcelToDTOMapper>().Run(assemblyName, files)
                       ? ("Successfully created the assembly")
                       : (String.Format("Error in processing. See {0} for details", DefaultSettings.LogFile));
        }
    }
}