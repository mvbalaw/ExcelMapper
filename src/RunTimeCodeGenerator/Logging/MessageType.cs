namespace RunTimeCodeGenerator.Logging
{
    public class MessageType
    {
        public static readonly MessageType Information = new MessageType("Information");
        public static readonly MessageType Warning = new MessageType("Warning");
        public static readonly MessageType Error = new MessageType("Error");

        public string Value { get; set; }

        private MessageType(string value)
        {
            Value = value;
        }
    }
}