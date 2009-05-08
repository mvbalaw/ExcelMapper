using System;
using System.Collections.Generic;

namespace ExcelMapper.DTO
{
    public class AssemblyProperties
    {
        public AssemblyProperties(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string FullName
        {
            get { return String.Format("{0}.dll", Name); }
        }

        public List<string> References = new List<string>();
        public List<string> Resources = new List<string>();

        public void AddReference(string reference)
        {
            References.Add(reference);
        }

        public void AddResource(string resource)
        {
            References.Add(resource);
        }
    }
}