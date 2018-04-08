using System.Collections.Generic;
using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.Stages;
using CompilerUtilities.Plugins.Versions;

namespace ExampleStages
{
    [Export(typeof(ILexer))]
    [Export(typeof(IPlugin))]
    public class Lexer:ILexer, IPlugin
    {
        public IList<IToken> Tokenize(ITextProcessor sourceCode)
        {
            _manager.Notify("Tokenize");

            return new List<IToken>();
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
            manager.OnParsed += (sender, args) => _manager.Notify("Проверка плагина");
        }
    }
}
