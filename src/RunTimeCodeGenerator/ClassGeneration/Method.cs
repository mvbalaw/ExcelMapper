using System.Collections.Generic;

namespace RunTimeCodeGenerator.ClassGeneration
{
    public class Method
    {
        public AccessLevel AccessLevel { get; set;}
        public string ReturnType { get; set; }
        public string Name { get; set; }

        public List<string> Body = new List<string>();

        public void AddBody(string line)
        {
            Body.Add(line);
        }
    }
}