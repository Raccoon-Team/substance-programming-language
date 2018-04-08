using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.Stages
{
    public interface IParser
    {
        ISyntaxTree Parse(IList<IToken> tokens);
    }
}