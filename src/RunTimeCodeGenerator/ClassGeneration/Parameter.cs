using System;

namespace RunTimeCodeGenerator.ClassGeneration
{
	public class Parameter
	{
		public Parameter(string type, string name)
		{
			Type = type;
			Name = name;
		}

		public Parameter()
		{
		}

		public string Name { get; set; }
		public string Type { get; set; }

		public override string ToString()
		{
			return String.Format("{0} {1}", Type, Name);
		}
	}
}