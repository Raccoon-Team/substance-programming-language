using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;
using IL2MSIL;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleTranslator : IStage<ITextProcessor, Blanket>
    {
        [Parameter("-output_file")]
        private string _outpFile;

        public Blanket Process(ITextProcessor input)
        {
            ILTranslator.Compile(Path.GetFileNameWithoutExtension(_outpFile), true, input.Presentation.ToList());
            //input.SaveToFile(outpFile);
            return null;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {

        }
    }
}