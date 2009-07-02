using System;

namespace RunTimeCodeGenerator.ClassGeneration
{
    public interface IClassFileWriter : IDisposable
    {
        void StartBlock();
        void CloseBlock();
        void Write(string value);
        void Write();
    }
}