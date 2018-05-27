using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages.Old
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IStage<,>))]
    public class ExampleFileReader : IStage<Blanket, ITextProcessor>
    {
        private IFileBuffer FileBuffer;
        [Parameter("-input_file")] private string inputFile;

        public ITextProcessor Process(Blanket input)
        {
            if (File.Exists(inputFile))
            {
                var source = new ExampleTextProcessor();
                source.Presentation = FileBuffer.GetLines(inputFile);
                return source;
            }
            throw new CompileException($"{nameof(ExampleFileReader)}: Файл \"{inputFile}\" не найден");
        }

        public void Initialize(IFileBuffer fileBuffer)
        {
            FileBuffer = fileBuffer;
        }
    }
}