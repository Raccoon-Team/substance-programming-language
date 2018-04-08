using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.EventArgs;
using CompilerUtilities.Plugins.Versions;

namespace CompilerUtilities.Preprocessor
{
    public class Preprocessor:IPlugin
    {
        public string Name { get; } = "Preprocessor";
        public string Author { get; } = "Kwelifin";

        public string Description { get; } =
            "Plugin for preprocessing source file. Support connecting directive add-ons";

        public uint Priority { get; } = 100;
        public VersionInfo Version { get; } = VersionInfo.Parse("a0.1");
        public VersionInfo RequreCompilerVersion { get; } = VersionInfo.Parse("a0.1");

        private IPluginManager _manager;

        public void Activate(IPluginManager manager)
        {
            _manager = manager;
            manager.OnFileReadEnd += Translate;
            
        }

        private void Translate(object sender, FileReaderEventArgs eventArgs)
        {
            var sourceCode = eventArgs.TextProcessor;
            
        }
    }
}
