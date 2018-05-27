using System.ComponentModel.Composition;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages.Old
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IStage<,>))]
    public class ExampleIntermediateGenerator : IStage<ISyntaxTree, ITextProcessor>
    {
        public uint Priority { get; }
        
        public ITextProcessor Process(ISyntaxTree input)
        {
            var outp = new ExampleTextProcessor {Presentation = new[] {input.ToString()}};
            return outp;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {

        }
    }
}