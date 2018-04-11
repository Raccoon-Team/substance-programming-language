﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class ExampleParser : IStage<IList<IToken>, ISyntaxTree>
    {
        public uint Priority { get; }
        public VersionInfo Version { get; }
        public string Name { get; }
        public string Author { get; }
        public string Description { get; }

        public void Initialize(ICompileOptions options)
        {
            throw new NotImplementedException();
        }

        public ISyntaxTree Process(IList<IToken> input)
        {
            var outp = new ExampleSyntaxTree();
            outp.Nodes.Add(new ExampleSyntaxTreeNode(input[0], null));

            var currentNode = outp.Nodes[0];

            for (var i = 1; i < input.Count; i++)
            {
                currentNode.Nodes.Add(new ExampleSyntaxTreeNode(input[i], currentNode));
                if (input[i].Type == "Operation" || input[i].Type == "Single Operation")
                {
                    currentNode = currentNode.Nodes.Last();
                }
                else if (input[i].Type == "Register")
                {
                    if (currentNode.Value.Type == "Single Operation")
                        currentNode = currentNode.Parent;
                    else if (currentNode.Value.Type == "Operation" && currentNode.Nodes.Count == 2)
                        currentNode = currentNode.Parent;
                }
            }

            return outp;
        }

        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }
    }
}