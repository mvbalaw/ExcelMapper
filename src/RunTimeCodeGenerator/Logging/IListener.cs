namespace RunTimeCodeGenerator.Logging
{
    public interface IListener
    {
        void Writeline(MessageType messageType, string message);
    }
}