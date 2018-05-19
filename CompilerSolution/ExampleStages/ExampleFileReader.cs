using System.ComponentModel.Composition;
using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleFileReader : IStage<Blanket, ITextProcessor>
    {
        public uint Priority { get; }

        [Parameter("-input_file")]
        private string inputFile;

        public void Initialize()
        {
            
        }

        public ITextProcessor Process(Blanket input)
        {
            return new ExampleTextProcessor(inputFile);
        }

        public ITextProcessor ReadFromFile(string path)
        {
            if (File.Exists(path))
            {
                var source = new CodeProcessor();
                source.LoadFromFile(path);
                return source;
            }
            throw new CompileException($"{nameof(ExampleFileReader)}: Файл \"{path}\" не найден");
        }
    }
}