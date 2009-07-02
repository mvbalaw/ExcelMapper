using System;
using System.Collections.Generic;

namespace RunTimeCodeGenerator.ClassGeneration
{
    public class ClassAttributes
    {
        public ClassAttributes(string name)
        {
            Name = name;
        }

        public string Namespace { get; set; }
        public string Name { get; set; }
        public string FullName
        {
            get { return String.Format("{0}.cs", Name); }
        }

        public List<string> UsingNamespaces = new List<string>();

        public void AddUsingNamespaces(string usingNamespace)
        {
            UsingNamespaces.Add(usingNamespace);
        }

        public List<Property> Properties = new List<Property>();

        public void AddProperty(Property property)
        {
            Properties.Add(property);
        }

        public List<Method> Methods = new List<Method>();

        public void AddMethod(Method method)
        {
            Methods.Add(method);
        }
    }
}