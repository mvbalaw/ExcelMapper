namespace RunTimeCodeGenerator.ClassGeneration
{
    public class Property
    {
        public Property()
        {
        }

        public Property(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Type { get; set; }
        public string Name { get; set; }
    }
}