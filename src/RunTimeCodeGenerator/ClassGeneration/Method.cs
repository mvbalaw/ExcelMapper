using System.Collections.Generic;

namespace RunTimeCodeGenerator.ClassGeneration
{
	public class Method
	{
		public Method()
		{
			Body = new List<string>();
			Parameters = new List<Parameter>();
		}

		public AccessLevel AccessLevel { get; set; }
		public List<string> Body { get; set; }
		public string Name { get; set; }
		public List<Parameter> Parameters { get; set; }
		public string ReturnType { get; set; }

		public void AddBody(string line)
		{
			Body.Add(line);
		}
	}
}