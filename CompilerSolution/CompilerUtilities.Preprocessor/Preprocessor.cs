using System;
using CompilerUtilities.Plugins.Contract;

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

        public void Initialize(IFileBuffer fileBuffer)
        {
            throw new NotImplementedException();
        }
    }
}