using System.ComponentModel.Composition;
using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleTranslator : IStage<ITextProcessor, Blanket>
    {
        [Parameter("-output_file")] private string outputFile;

        public uint Priority { get; }

        public void Initialize()
        {
        }

        public Blanket Process(ITextProcessor input)
        {
            File.WriteAllLines(outputFile, input.Presentation);

            return null;
        }
    }
}