using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleLexer : IStage<ITextProcessor, IList<IToken>>
    {
        private readonly string[] ops = {"mov"};
        private readonly string[] singleOps = {"add", "mul", "div", "sub", "jmp"};
        public uint Priority { get; }

        public void Initialize()
        {
        }

        public IList<IToken> Process(ITextProcessor input)
        {
            var accum = new StringBuilder();
            var outp = new List<IToken>();

            var code = input.Presentation.Aggregate("", (s, s1) => s + s1);
            var length = code.Length;
            for (var i = 0; i < length; i++)
            {
                if (code[i] == ' ')
                {
                    accum.Clear();
                    while (i < length - 1 && code[i] == ' ')
                        i++;
                }
                if (i + 1 == length) break;

                accum.Append(code[i]);
                var accumStr = accum.ToString();

                if (ops.Contains(accumStr))
                {
                    outp.Add(new ExampleToken(accumStr, "Operation"));
                    accum.Clear();
                }
                else if (singleOps.Contains(accumStr))
                {
                    outp.Add(new ExampleToken(accumStr, "Single Operation"));
                    accum.Clear();
                }
                else if (Regex.IsMatch(accumStr, @"^e?(([a-d]x)|([sd]i))$"))
                {
                    outp.Add(new ExampleToken(accumStr, "Register"));
                    accum.Clear();
                }
                else if (accumStr == ",")
                {
                    outp.Add(new ExampleToken(",", "Comma"));
                    accum.Clear();
                }
            }

            return outp;
        }
    }
}