using System;

namespace BuildDTOsFromExcel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
                return;
            }

            if (args[0] == "/?" || args[0] == "--help")
            {
                DisplayUsage();
                return;
            }

            Console.WriteLine(new Engine().Run(args));
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Maps non-empty workSheets in excel files to an assembly containing strongly typed objects");
            Console.WriteLine();
            Console.WriteLine("BuildDTOsFromExcel   [/Assembly[[:]assemblyName]] [Excel Files]");
            Console.WriteLine("/Assembly            [Optional] Assembly name");
            Console.WriteLine("[Excel Files]        Excel files (*.xls, *.xlsx) that has to be mapped");
            Console.WriteLine();
            Console.WriteLine("Example Usage1: BuildDTOsFromExcel file1.xls file2.xlsx ...");
            Console.WriteLine("Example Usage2: BuildDTOsFromExcel /Assembly:MyAssembly file1.xls file2.xlsx ...");
            Console.WriteLine("Example Usage3: BuildDTOsFromExcel /Assembly:MyAssembly *.xls *.xlsx");
        }
    }
}