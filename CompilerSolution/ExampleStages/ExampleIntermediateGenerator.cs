using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
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