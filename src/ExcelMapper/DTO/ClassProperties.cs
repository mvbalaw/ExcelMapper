using System;
using System.Collections.Generic;

namespace ExcelMapper.DTO
{
    public class ClassProperties
    {
        public ClassProperties(string name)
        {
            Name = name;
        }

        
        public string Name { get; set; }
        public string FullName
        {
            get { return String.Format("{0}.cs", Name); }
        }
        public string NameSpace { get; set; }

        public List<string> Property = new List<string>();
        public List<string> PropertyType = new List<string>();

        public void AddProperty(string name)
        {
            Property.Add(name);
        }

        public void AddPropertyType(string type)
        {
            PropertyType.Add(type);
        }
    }
}