using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using ExcelMapper.Configuration;
using ExcelMapper.Repository.Connection;
using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public class OleDbDataProvider : IDataProvider
    {
        private readonly IConnectionBuilder _connectionBuilder;
        private readonly IFileConfiguration _fileConfiguration;

        public OleDbDataProvider(IConnectionBuilder connectionBuilder, IFileConfiguration fileConfiguration)
        {
            _connectionBuilder = connectionBuilder;
            _fileConfiguration = fileConfiguration;
        }

        public IEnumerable<string> GetTableNames()
        {
            using (OleDbConnection connection = _connectionBuilder.GetConnection(_fileConfiguration.FileName))
            {
                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        yield return schemaTable.Rows[i]["TABLE_NAME"].ToString();
                    }
                }
            }
        }

        public IEnumerable<Property> GetColumns(string workSheet)
        {
            using (OleDbConnection connection = _connectionBuilder.GetConnection(_fileConfiguration.FileName))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT * FROM [{0}]", workSheet);
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                yield return new Property(reader.GetFieldType(i).ToString(),
                                                          reader.GetName(i));
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<T> Get<T>(string workSheet)
        {
            using (OleDbConnection connection = _connectionBuilder.GetConnection(_fileConfiguration.FileName))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT * FROM [{0}$]", workSheet);
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        PropertyInfo[] properties = typeof (T).GetProperties();

                        while (reader.Read())
                        {
                            var instance = Activator.CreateInstance<T>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int index = i;
                                PropertyInfo property = properties.Single(p => p.Name.Equals(reader.GetName(index)));
                                property.SetValue(instance, reader.GetValue(i), null);
                            }
                            yield return instance;
                        }
                    }
                }
            }
        }

        public void Put<T>(IEnumerable<T> values)
        {
            PropertyInfo[] properties = typeof (T).GetProperties();
            string query = String.Format("INSERT INTO [{0}$] VALUES ({1})", typeof (T).Name,
                                         ConstructQueryValues(properties.Length));
            using (OleDbConnection connection = _connectionBuilder.GetConnection(_fileConfiguration.FileName))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    foreach (T value in values)
                    {
                        foreach (PropertyInfo property in properties)
                        {
                            command.Parameters.AddWithValue(property.Name, property.GetValue(value, null));
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CreateTable<T>()
        {
            PropertyInfo[] properties = typeof (T).GetProperties();
            var tableColumns = new StringBuilder();
            const string separator = ", ";

            foreach (PropertyInfo property in properties)
            {
                tableColumns.AppendFormat("{0} {1}{2}", property.Name, property.PropertyType.GetPropertyType(),
                                          separator);
            }
            tableColumns.Remove(tableColumns.Length - separator.Length, separator.Length);

            string query = String.Format("CREATE TABLE {0} ({1})", typeof (T).Name, tableColumns);
            using (OleDbConnection connection = _connectionBuilder.GetConnection(_fileConfiguration.FileName))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string ConstructQueryValues(int length)
        {
            var query = new StringBuilder();
            for (int i = 0; i < length - 1; i++)
            {
                query.Append("?, ");
            }
            query.Append("?");
            return query.ToString();
        }
    }

    public static class PropertyTypeExtensions
    {
        public static string GetPropertyType(this Type propertyType)
        {
            const string integerType = "Int";
            return propertyType.Name.Contains(integerType) ? integerType : propertyType.Name;
        }
    }
}