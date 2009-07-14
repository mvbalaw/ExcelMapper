using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

using ExcelMapper.Configuration;
using ExcelMapper.Repository.Connection;
using ExcelMapper.Repository.Extensions;

using RunTimeCodeGenerator.ClassGeneration;

namespace ExcelMapper.Repository
{
    public class ExcelRepository : IRepository
    {
        private readonly IConnectionBuilder _connectionBuilder;
        private readonly IFileConfiguration _fileConfiguration;

        public ExcelRepository(IConnectionBuilder connectionBuilder, IFileConfiguration fileConfiguration)
        {
            _connectionBuilder = connectionBuilder;
            _fileConfiguration = fileConfiguration;
        }

        public IEnumerable<string> GetWorkSheetNames()
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

        public ClassAttributes GetClassAttributes(string workSheet)
        {
            string className = workSheet.Replace("$", "");
            ClassAttributes classAttributes = new ClassAttributes(className);

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
                                classAttributes.Properties.Add(new Property(reader.GetFieldType(i).ToString(), reader.GetName(i)));
                            }
                        }
                    }
                }
            }
            return classAttributes;
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
                        PropertyInfo[] properties = typeof(T).GetProperties();

                        while (reader.Read())
                        {
                            T instance = Activator.CreateInstance<T>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int index = i;
                                PropertyInfo property = properties.Single(p => p.Name.Equals(reader.GetName(index)));
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