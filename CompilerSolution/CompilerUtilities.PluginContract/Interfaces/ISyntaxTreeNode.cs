using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract.Interfaces
{
    public interface ISyntaxTreeNode
    {
        IToken Value { get; }
        IList<ISyntaxTreeNode> Nodes { get; set; }
        ISyntaxTreeNode Parent { get; }
    }
}