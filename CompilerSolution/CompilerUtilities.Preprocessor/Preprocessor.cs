using System;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace CompilerUtilities.Preprocessor
{
    public class Preprocessor : IPlugin<ITextProcessor>
    {
        public uint Priority { get; } = 100;
        public string Name { get; } = "Preprocessor";
        public string Author { get; } = "Kwelifin";

        public string Description { get; } =
            "Plugin for preprocessing source file. Support connecting directive add-ons";

        public ITextProcessor Activate(ITextProcessor input)
        {
            throw new NotImplementedException();
        }

        public VersionInfo Version { get; } = VersionInfo.Parse("a0.1");
        public VersionInfo RequreCompilerVersion { get; } = VersionInfo.Parse("a0.1");
    }
}