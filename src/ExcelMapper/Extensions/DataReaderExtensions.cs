using System.Data;

namespace ExcelMapper.Extensions
{
    public static class DataReaderExtensions
    {
        public static T GetValue<T>(this IDataReader reader, int ordinal)
        {
            if (!reader.IsDBNull(ordinal))
            {
                return (T)reader.GetValue(ordinal);
            }
            return default(T);
        }
    }
}