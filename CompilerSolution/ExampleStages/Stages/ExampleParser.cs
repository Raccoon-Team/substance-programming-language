using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleParser : IStage<IList<IToken>, ISyntaxTree>
    {
        public string Name { get; }
        public string Author { get; }
        public string Description { get; }
        public uint Priority { get; }

        public void Initialize()
        {
        }

        public ISyntaxTree Process(IList<IToken> input)
        {
            var outp = new ExampleSyntaxTree();
            outp.Nodes.Add(new ExampleSyntaxTreeNode(null, null));

            var currentNode = outp.Nodes[0];

            for (var i = 0; i < input.Count; i++)
            {
                var token = input[i];

                if (new[] {"mov", "add", "sub"}.Contains(token.Value))
                {
                    var node = new ExampleSyntaxTreeNode(input[i++], currentNode);
                    currentNode.Nodes.Add(node);
                    node.Nodes.Add(new ExampleSyntaxTreeNode(input[i++], node));
                    node.Nodes.Add(new ExampleSyntaxTreeNode(input[++i], node));
                }
                else if (new[] { "mul", "div", "inc", "dec" }.Contains(token.Value))
                {
                    var node = new ExampleSyntaxTreeNode(input[i++], currentNode);
                    currentNode.Nodes.Add(node);
                    node.Nodes.Add(new ExampleSyntaxTreeNode(input[i], node));
                }
            }

            //for (var i = 1; i < input.Count; i++)
            //{
            //    //if (input[i].Type == "Comma") continue;
            //    currentNode.Nodes.Add(new ExampleSyntaxTreeNode(input[i], currentNode));
            //    if (input[i].Type == "Operation" || input[i].Type == "Single Operation")
            //        currentNode = currentNode.Nodes.Last();
            //    else if (input[i].Type == "Register")
            //        if (currentNode.Value.Type == "Single Operation")
            //            currentNode = currentNode.Parent;
            //        else if (currentNode.Value.Type == "Operation" && currentNode.Nodes.Count > 2)
            //            currentNode = currentNode.Parent;
            //}

            return outp;
        }
    }
}