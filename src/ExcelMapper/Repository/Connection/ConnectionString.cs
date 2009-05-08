using System;
using System.IO;

namespace ExcelMapper.Repository.Connection
{
    public class ConnectionString : IConnectionString
    {
        public string Get(string file)
        {
            string fileExtension = Path.GetExtension(file);

            if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return CreateConnectionStringWithDataSource("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"", file);
            }
            if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
            {
                return CreateConnectionStringWithDataSource("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES;\"", file);
            }
            throw new ArgumentException("File Type not supported");
        }

        private static string CreateConnectionStringWithDataSource(string connectionString, string file)
        {
            return String.Format(connectionString, file);
        }
    }
}