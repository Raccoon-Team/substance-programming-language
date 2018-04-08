using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.Stages
{
    public interface ILexer
    {
        IList<IToken> Tokenize(ITextProcessor sourceCode);
    }
}