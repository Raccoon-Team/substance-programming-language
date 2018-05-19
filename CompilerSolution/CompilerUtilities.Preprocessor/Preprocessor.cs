using System;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Interfaces;
using CompilerUtilities.Plugins.Contract.Versions;

namespace CompilerUtilities.Preprocessor
{
    [RequiredCompilerVersion("a0.1")]
    public class Preprocessor : IPlugin<ITextProcessor>
    {
        public uint Priority { get; } = 100;

        public ITextProcessor Activate(ITextProcessor input)
        {
            throw new NotImplementedException();
        }
    }
}