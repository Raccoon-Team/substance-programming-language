using System.Collections.Generic;
using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.Stages;
using CompilerUtilities.Plugins.Versions;

namespace ExampleStages
{
    public class ExampleSyntaxTree : ISyntaxTree
    { }

    [Export(typeof(IParser))]
    [Export(typeof(IPlugin))]
    public class Parser:IParser, IPlugin
    {
        public ISyntaxTree Parse(IList<IToken> tokens)
        {
            _manager.Notify("Parse");
            return new ExampleSyntaxTree();
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