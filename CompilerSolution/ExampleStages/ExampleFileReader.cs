using System.ComponentModel.Composition;
using System.IO;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class ExampleFileReader : IStage<Blanket, ITextProcessor>
    {
        public uint Priority { get; }

        public string Name { get; }
        public string Author { get; }
        public string Description { get; }

        public void Initialize(ICompileOptions options)
        {
            throw new System.NotImplementedException();
        }

        public ITextProcessor Process(Blanket input)
        {
            throw new System.NotImplementedException();
        }

        public VersionInfo Version { get; }
        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }

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