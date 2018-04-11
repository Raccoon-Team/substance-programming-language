using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class GeneratorIntermediate : IStage<ISyntaxTree, ITextProcessor>
    {
        public uint Priority { get; }

        public string Name { get; }
        public string Author { get; }
        public string Description { get; }
        public void Initialize(ICompileOptions options)
        {
            throw new System.NotImplementedException();
        }

        public ITextProcessor Process(ISyntaxTree input)
        {
            throw new System.NotImplementedException();
        }

        public VersionInfo Version { get; }
        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }
        
    }
}