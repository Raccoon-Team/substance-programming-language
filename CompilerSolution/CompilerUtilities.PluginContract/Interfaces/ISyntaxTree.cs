using System.Collections.Generic;

namespace CompilerUtilities.Plugins.Contract.Interfaces
{
    public interface ISyntaxTree
    {
        IList<ISyntaxTreeNode> Nodes { get; }
    }
}