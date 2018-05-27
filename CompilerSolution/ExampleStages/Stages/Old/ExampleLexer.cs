using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages.Old
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IStage<,>))]
    public class ExampleLexer : IStage<ITextProcessor, IList<IToken>>
    {
        private TokenTypesCollection _tokenTypesCollection;
        public uint Priority { get; }
        
        public void Initialize(IFileBuffer fileBuffer)
        {
            _tokenTypesCollection = new TokenTypesCollection();
            _tokenTypesCollection.Initialize();
        }

        public IList<IToken> Process(ITextProcessor input)
        {
            return Tokenize(input.Presentation);

            /*var accum = new StringBuilder();
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

            return outp;*/
        }

        private List<IToken> Tokenize(IEnumerable<string> input)
        {
            var accum = new StringBuilder();
            var outp = new List<IToken>();

            var isString = false;
            foreach (var line in input)
            {
                var length = line.Length;


                for (var i = 0; i < length; i++)
                {
                    if (char.IsWhiteSpace(line[i]) && !isString)
                    {
                        accum.Clear();
                        while (++i < length && char.IsWhiteSpace(line[i]))
                        {
                        }
                        if (i == length - 1) break;
                    }

                    do
                    {
                        char character;
                        if (i == length || char.IsWhiteSpace(character = line[i]) && !isString)
                        {
                            break;
                        }

                        if (accum.Length > 0 &&
                            !_tokenTypesCollection.CompareCharCategory(character, accum[accum.Length - 1]))
                        {
                            i--;
                            break;
                        }

                        if (character == '"')
                        {
                            isString = !isString;
                            if (!isString)
                                break;
                        }

                        accum.Append(character);
                        i++;
                    } while (true);
                    
                    if (isString) continue;

                    var strAccum = accum.ToString();
                    accum.Clear();
                    outp.Add(new ExampleToken(strAccum, _tokenTypesCollection[strAccum]));
                }
            }

            var cw = string.Join("\r\n", outp.Select(t => $"{t.Value}:{t.Type}"));
            Console.WriteLine(cw);

            return outp;
        }
    }
}