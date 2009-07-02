namespace RunTimeCodeGenerator.AssemblyGeneration
{
    public interface IAssemblyGenerator
    {
        bool Compile(string[] classNames, AssemblyAttributes assemblyAttributes);
    }
}