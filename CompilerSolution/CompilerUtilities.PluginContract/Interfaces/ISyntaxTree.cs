using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract
{
    public interface ISyntaxTree
    {
        IList<ISyntaxTreeNode> Nodes { get; }
    }
}