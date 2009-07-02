using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildDTOsFromExcel
{
    public static class Extensions
    {
        private const string AssemblyPredicate = "/Assembly:";

        public static List<string> GetFiles(this string[] args)
        {
            return Array.FindAll(args, arg => !arg.Contains(AssemblyPredicate)).ToList();
        }

        public static string GetAssemblyName(this string[] args)
        {
            string assembly = Array.Find(args, arg => arg.Contains(AssemblyPredicate));
            return !String.IsNullOrEmpty(assembly) ? assembly.Replace(AssemblyPredicate, String.Empty) : DefaultSettings.AssemblyName;
        }
    }
}