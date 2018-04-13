using System.Collections.Generic;
using CompilerUtilities.BaseTypes.Interfaces;

namespace ExampleStages.ExampleTypes
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

        public override string ToString()
        {
            return $"{Value.Type}: {Value.Value}\r\n{string.Join("\r\n", Nodes)}";
        }
    }
}