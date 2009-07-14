using System;
using System.Collections.Generic;
using System.Data.OleDb;

using ExcelMapper.Repository.Connection;
using ExcelMapper.Repository.Extensions;
using ExcelMapper.Tests.DTO;

namespace ExcelMapper.Tests
{
    public class TestData
    {
        public static string UsersXlsx = @"Excel\UsersXlsx.xlsx";
        public static string UsersXls = @"Excel\UsersXls.xls";
        public static string UsersXlsxConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Excel\\UsersXlsx.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";
        public static string UsersXlsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Excel\\UsersXls.xls;Extended Properties=\"Excel 8.0;HDR=YES;\"";
        public static string AssemblyName = "TestAssembly";
        public static string LogsDirectory = "Logs";

        public static IEnumerable<User> GetUsers(string file, string workSheet)
        {
            using (OleDbConnection connection = new ConnectionBuilder(new ConnectionString()).GetConnection(file))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [User$]";
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                Id = reader.GetValue<double>(0),
                                LastName = reader.GetValue<string>(1),
                                FirstName = reader.GetValue<string>(2),
                                DateOfBirth = reader.GetValue<DateTime>(3)
                            };
                            yield return user;
                        }
                    }
                }
            }
        }
    }
}