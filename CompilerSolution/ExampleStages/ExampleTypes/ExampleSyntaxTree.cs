using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace ExampleStages
{
    public class ExampleSyntaxTree : ISyntaxTree
    {
        public ExampleSyntaxTree()
        {
            Nodes = new List<ISyntaxTreeNode>();
        }

        public IList<ISyntaxTreeNode> Nodes { get; }
    }
}