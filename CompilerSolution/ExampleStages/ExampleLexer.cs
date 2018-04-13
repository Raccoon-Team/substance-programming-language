using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;
using ExampleStages.ExampleTypes;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class ExampleLexer : IStage<ITextProcessor, IList<IToken>>
    {
        public uint Priority { get; }
        public VersionInfo Version { get; }
        public string Name { get; }
        public string Author { get; }
        public string Description { get; }

        public void Initialize(ICompileOptions options)
        {
            
        }

        private readonly string[] ops = {"mov"};
        private readonly string[] singleOps = {"add", "mul", "div", "sub", "jmp"};

        public IList<IToken> Process(ITextProcessor input)
        {
            var accum = new StringBuilder();
            var outp = new List<IToken>();

            var code = input.Presentation.Aggregate("", (s, s1) => s + s1);

            for (var i = 0; i < code.Length; i++)
            {
                if (code[i] == ' ')
                {
                    accum.Clear();
                    while (code[i] == ' ')
                        i++;
                }
                accum.Append(code[i]);

                //Match match;
                if (ops.Contains(accum.ToString()))
                {
                    outp.Add(new ExampleToken(accum.ToString(), "Operation"));
                    accum.Clear();
                }
                else if (singleOps.Contains(accum.ToString()))
                {
                    outp.Add(new ExampleToken(accum.ToString(), "Single Operation"));
                    accum.Clear();
                }
                else if (Regex.IsMatch(accum.ToString(), @"^e?(([a-d]x)|([sd]i))$"))
                {
                    outp.Add(new ExampleToken(accum.ToString(), "Register"));
                    accum.Clear();
                }
                else if (accum.ToString() == ",")
                {
                    outp.Add(new ExampleToken(",", "Comma"));
                    accum.Clear();
                }
            }

            return outp;
        }

        public VersionInfo VersionInfo { get; }
        public VersionInfo RequreCompilerVersion { get; }
    }
}