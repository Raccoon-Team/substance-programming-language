using System.ComponentModel.Composition;
using System.IO;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class ExampleFileReader : IStage<Blanket, ITextProcessor>
    {
        public uint Priority { get; }

        private ICompileOptions _options;

        public void Initialize(ICompileOptions options)
        {
            _options = options;
        }

        public ITextProcessor Process(Blanket input)
        {
            var fileName = _options["input_file"];
            return new ExampleTextProcessor(fileName);
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