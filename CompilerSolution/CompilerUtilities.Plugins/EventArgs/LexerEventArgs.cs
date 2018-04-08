using System.Collections;
using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.EventArgs
{
    public class LexerEventArgs:System.EventArgs
    {
        public IList<IToken> Tokens;

        public LexerEventArgs(IList<IToken> tokens)
        {
            Tokens = tokens;
        }
    }
}