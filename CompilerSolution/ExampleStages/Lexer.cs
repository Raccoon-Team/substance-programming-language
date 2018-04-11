using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class Lexer : IStage<ITextProcessor, IList<IToken>>
    {
        public uint Priority { get; }
        public VersionInfo Version { get; }
        public string Name { get; }
        public string Author { get; }
        public string Description { get; }

        public void Initialize(ICompileOptions options)
        {
            throw new NotImplementedException();
        }

        public IList<IToken> Process(ITextProcessor input)
        {
            throw new NotImplementedException();
        }

        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }
    }
}