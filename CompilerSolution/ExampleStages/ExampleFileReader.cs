using System.ComponentModel.Composition;
using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Interfaces;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleFileReader : IStage<Blanket, ITextProcessor>
    {
        [Parameter("-input_file")] private string inputFile;

        public uint Priority { get; }

        public void Initialize()
        {
        }

        public ITextProcessor Process(Blanket input)
        {
            if (File.Exists(inputFile))
            {
                var source = new ExampleTextProcessor();
                source.LoadFromFile(inputFile);
                return source;
            }
            throw new CompileException($"{nameof(ExampleFileReader)}: Файл \"{inputFile}\" не найден");
        }
    }
}