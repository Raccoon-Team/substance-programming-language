using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace ExampleStages
{
    public class ExampleSyntaxTreeNode : ISyntaxTreeNode
    {
        public ExampleSyntaxTreeNode(IToken value, ISyntaxTreeNode parent)
        {
            Value = value;
            Parent = parent;
            Nodes = new List<ISyntaxTreeNode>();
        }

        public IToken Value { get; }
        public IList<ISyntaxTreeNode> Nodes { get; set; }
        public ISyntaxTreeNode Parent { get; }
    }
}