using System.ComponentModel.Composition;
using CompilerUtilities.Plugins.Contract.Interfaces;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleIntermediateGenerator : IStage<ISyntaxTree, ITextProcessor>
    {
        public uint Priority { get; }

        public void Initialize()
        {
        }

        public ITextProcessor Process(ISyntaxTree input)
        {
            var outp = new ExampleTextProcessor {Presentation = new[] {input.ToString()}};
            return outp;
        }
    }
}