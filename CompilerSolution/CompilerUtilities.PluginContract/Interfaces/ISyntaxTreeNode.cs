using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract
{
    public interface ISyntaxTreeNode
    {
        IToken Value { get; }
        IList<ISyntaxTreeNode> Nodes { get; set; }
        ISyntaxTreeNode Parent { get; }
    }
}