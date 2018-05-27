using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages.Old
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IStage<,>))]
    public class ExampleParser : IStage<IList<IToken>, ISyntaxTree>
    {
        public uint Priority { get; }
        
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

            return outp;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {
            throw new System.NotImplementedException();
        }
    }
}