using System.Collections.Generic;

namespace CompilerUtilities.BaseTypes.Interfaces
{
    public interface ISyntaxTreeNode
    {
        IToken Value { get; }
        IList<ISyntaxTreeNode> Nodes { get; set; }
        ISyntaxTreeNode Parent { get; }
    }
}