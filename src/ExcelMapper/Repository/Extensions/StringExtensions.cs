using System;

namespace ExcelMapper.Repository.Extensions
{
    public static class StringExtensions
    {
        public static string GetClassName(this string value)
        {
            return value.Replace("$", "");
        }
        
        public static string GetWorkSheetName(this string value)
        {
            return String.Format("{0}$", value);
        }
    }
}