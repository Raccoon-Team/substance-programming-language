using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins;
using CompilerUtilities.Plugins.Stages;
using CompilerUtilities.Plugins.Versions;

namespace ExampleStages
{
    [Export(typeof(IGeneratorIntermediate))]
    [Export(typeof(IPlugin))]
    public class GeneratorIntermediate:IGeneratorIntermediate, IPlugin
    {
        public ITextProcessor Generate(ISyntaxTree tree)
        {
            _manager.Notify("Generate");
            return new CodeProcessor();
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