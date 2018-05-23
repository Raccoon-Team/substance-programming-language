using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CompilerUtilities.Plugins.Contract.Interfaces;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
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
            outp.Nodes.Add(new ExampleSyntaxTreeNode(input[0], null));

            var currentNode = outp.Nodes[0];

            for (var i = 1; i < input.Count; i++)
            {
                //if (input[i].Type == "Comma") continue;
                currentNode.Nodes.Add(new ExampleSyntaxTreeNode(input[i], currentNode));
                if (input[i].Type == "Operation" || input[i].Type == "Single Operation")
                    currentNode = currentNode.Nodes.Last();
                else if (input[i].Type == "Register")
                    if (currentNode.Value.Type == "Single Operation")
                        currentNode = currentNode.Parent;
                    else if (currentNode.Value.Type == "Operation" && currentNode.Nodes.Count > 2)
                        currentNode = currentNode.Parent;
            }

            return outp;
        }
    }
}