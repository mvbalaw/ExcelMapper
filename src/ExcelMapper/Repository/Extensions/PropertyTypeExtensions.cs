using System;

namespace ExcelMapper.Repository.Extensions
{
    public static class PropertyTypeExtensions
    {
        public static string GetPropertyType(this Type propertyType)
        {
            const string integerType = "Int";
            return propertyType.Name.Contains(integerType) ? integerType : propertyType.Name;
        }
    }
}