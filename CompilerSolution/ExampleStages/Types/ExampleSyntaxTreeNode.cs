using System.Collections.Generic;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Types
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
            return $"{Value.Type}: {Value.Value}\r\n\t{string.Join("\r\n\t", Nodes)}";
        }
    }
}