namespace RunTimeCodeGenerator.ClassGeneration
{
    public class AccessLevel
    {
        public static readonly AccessLevel Private = new AccessLevel("private");
        public static readonly AccessLevel Protected = new AccessLevel("protected");
        public static readonly AccessLevel Public = new AccessLevel("public");

        private AccessLevel(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}