using System.Collections.Generic;

namespace CompilerUtilities.BaseTypes.Interfaces
{
    public interface ISyntaxTree
    {
        IList<ISyntaxTreeNode> Nodes { get; }
    }
}