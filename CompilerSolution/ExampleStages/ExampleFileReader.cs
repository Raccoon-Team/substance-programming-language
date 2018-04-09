using System.ComponentModel.Composition;
using System.IO;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.Stages;
using CompilerUtilities.Plugins.Versions;

namespace ExampleStages
{
    [Export(typeof(IFileReader))]
    [Export(typeof(IPlugin))]
    public class ExampleFileReader : IFileReader, IPlugin
    {
        private IPluginManager _manager;

        public ITextProcessor ReadFromFile(string path)
        {
            _manager.Notify("ReadFromFile");
            if (File.Exists(path))
            {
                var source = new CodeProcessor();
                source.LoadFromFile(path);
                return source;
            }
            throw new CompileException($"{nameof(ExampleFileReader)}: Файл \"{path}\" не найден");
        }

        public string Name { get; }
        public string Author { get; }
        public string Description { get; }
        public uint Priority { get; }
        public VersionInfo Version { get; }
        public VersionInfo RequreCompilerVersion { get; }

        public void Activate(IPluginManager manager)
        {
            _manager = manager;
        }
    }
}