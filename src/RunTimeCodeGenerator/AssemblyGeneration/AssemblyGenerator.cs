using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

using log4net;
using log4net.Config;

using Microsoft.CSharp;

namespace RunTimeCodeGenerator.AssemblyGeneration
{
    public class AssemblyGenerator : IAssemblyGenerator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssemblyGenerator));

        public bool Compile(string[] classNames, AssemblyAttributes assemblyAttributes)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(new Dictionary<string, string>
                {
                    { "CompilerVersion", "v3.5" }
                });

            CompilerParameters parameters = new CompilerParameters
                {
                    OutputAssembly = assemblyAttributes.FullName
                };

            parameters.ReferencedAssemblies.AddRange(assemblyAttributes.References.ToArray());
            parameters.EmbeddedResources.AddRange(assemblyAttributes.Resources.ToArray());

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromFile(parameters, classNames);

            GenerateErrorReport(compilerResults.Errors);

            return compilerResults.Errors.Count == 0;
        }

        private static void GenerateErrorReport(CompilerErrorCollection errorsCollection)
        {
            XmlConfigurator.Configure(new FileInfo(DefaultSettings.LogFile));

            if (errorsCollection.Count == 0)
            {
                return;
            }
            foreach (CompilerError error in errorsCollection)
            {
                Log.ErrorFormat("{0}: {1}", error.FileName, error.ErrorText);
            }
        }
    }
}