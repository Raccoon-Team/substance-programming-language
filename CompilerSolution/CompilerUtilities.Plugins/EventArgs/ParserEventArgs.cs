using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.EventArgs
{
    public class ParserEventArgs:System.EventArgs
    {
        public ISyntaxTree Tree;

        public ParserEventArgs(ISyntaxTree tree)
        {
            Tree = tree;
        }
    }
}