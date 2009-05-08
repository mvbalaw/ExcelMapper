using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

using ExcelMapper.DTO;
using ExcelMapper.Extension;
using ExcelMapper.Repository.Connection;

namespace ExcelMapper.Repository
{
    public class ExcelRepository : IRepository
    {
        private readonly IConnection _connection;

        public ExcelRepository(IConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<string> GetWorkSheetNames(string fileName)
        {
            using (OleDbConnection connection = _connection.GetConnection(fileName))
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

        public ClassProperties GetClassProperties(string file, string workSheet)
        {
            string className = workSheet.Replace("$", "");
            ClassProperties classProperties = new ClassProperties(className);

            using (OleDbConnection connection = _connection.GetConnection(file))
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
                                classProperties.Property.Add(reader.GetName(i));
                                classProperties.PropertyType.Add(reader.GetFieldType(i).ToString());
                            }
                        }
                    }
                }
            }
            return classProperties;
        }

        public IEnumerable<T> Get<T>(string file, string workSheet)
        {
            using (OleDbConnection connection = _connection.GetConnection(file))
            {
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format("SELECT * FROM [{0}$]", workSheet);
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        PropertyInfo[] properties = typeof(T).GetProperties();

                        while (reader.Read())
                        {
                            T instance = Activator.CreateInstance<T>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PropertyInfo property = properties.Single(p => p.Name.Equals(reader.GetName(i)));
                                property.SetValue(instance, reader.GetValue<object>(i), null);
                            }
                            yield return instance;
                        }
                    }
                }
            }
        }
    }
}