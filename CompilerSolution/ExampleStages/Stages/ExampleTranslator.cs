using System.ComponentModel.Composition;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleTranslator : IStage<ITextProcessor, Blanket>
    {
        [Parameter("-output_file")]
        private string outpFile;

        public Blanket Process(ITextProcessor input)
        {
            input.SaveToFile(outpFile);
            return null;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {

        }
    }
}