using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.Stages;
using CompilerUtilities.Plugins.Versions;

namespace ExampleStages
{
    [Export(typeof(ITranslator))]
    [Export(typeof(IPlugin))]
    public class ExampleTranslator:ITranslator, IPlugin
    {
        public void Translate(ITextProcessor intermediateCode, string outputPath)
        {
            _manager.Notify("Translate");
        }

        public string Name { get; }
        public string Author { get; }
        public string Description { get; }
        public uint Priority { get; }
        public VersionInfo Version { get; }
        public VersionInfo RequreCompilerVersion { get; }

        private IPluginManager _manager;

        public void Activate(IPluginManager manager)
        {
            _manager = manager;
        }
    }
}