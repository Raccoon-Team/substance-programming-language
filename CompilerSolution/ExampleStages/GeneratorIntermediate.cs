using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

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
        }

        public ITextProcessor Process(ISyntaxTree input)
        {
            var outp = new ExampleTextProcessor {Presentation = new[] {input.ToString()}};
            return outp;
        }

        public VersionInfo Version { get; }
        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }
        
    }
}