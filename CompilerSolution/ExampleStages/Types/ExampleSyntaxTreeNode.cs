using System;
using System.Collections.Generic;
using System.Text;
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
            if (Value is null)
                return $"Entry point:\r\n{string.Join("\r\n", Nodes)}";

            var parents = 0;
            ISyntaxTreeNode currNode = this;
            while (currNode.Parent != null)
            {
                parents++;
                currNode = currNode.Parent;
            }

            var tab = new string('\t', parents);
            return $"{tab}{Value.Type}: {Value.Value}\r\n{string.Join("\r\n", Nodes)}";
        }
    }
}