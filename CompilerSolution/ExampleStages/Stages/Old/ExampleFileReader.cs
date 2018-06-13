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
        [Parameter("-input_file")] private string _inputFile;

        public ITextProcessor Process(Blanket input)
        {
            if (File.Exists(_inputFile))
            {
                var source = new ExampleTextProcessor {Presentation = FileBuffer.GetLines(_inputFile)};
                return source;
            }
            throw new FileNotFoundException($"{nameof(ExampleFileReader)}: Файл \"{_inputFile}\" не найден");
        }

        public void Initialize(IFileBuffer fileBuffer)
        {
            FileBuffer = fileBuffer;
        }
    }
}