using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleConstructionsInliner : IStage<IList<ConstructionInfo>, ITextProcessor>
    {
        [Parameter("-input_file")] private string sourceFileName;

        public void Initialize(IFileBuffer fileBuffer)
        {
        }

        public ITextProcessor Process(IList<ConstructionInfo> input)
        {
            var src = new StringBuilder(File.ReadAllText(sourceFileName));
            foreach (var info in input)
                ReplaceConstruction(src, info);

            return new ExampleTextProcessor(new []{src.ToString()});
        }

        private static bool CheckSymbol(StringBuilder input, char pattern, ref int inpIndex)
        {
            if (char.IsWhiteSpace(input[inpIndex]) && char.IsWhiteSpace(pattern))
            {
                while (char.IsWhiteSpace(input[inpIndex + 1]))
                    inpIndex++;
                return true;
            }

            return input[inpIndex] == pattern;
        }

        private static void ReplaceConstruction(StringBuilder input, ConstructionInfo construction)
        {
            var inputLength = input.Length;

            var parts = SplitConstruction(construction);
            var implementation = construction.Implementation;
            var parameters = construction.Parameters;

            int constructionStart = -1, constructionLength = parts.Sum(s => s.Length);
            // ReSharper disable once TooWideLocalVariableScope
            int start, end = 0, lastEnd = 0;

            for (var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                bool success;
                do
                {
                    start = end;
                    while (start < inputLength && !CheckSymbol(input, part[0], ref start))
                    {
                        start++;
                        if (start == inputLength)
                            return;
                    }
                    if (i == 0)
                        constructionStart = start;
                    //----------------------
                    if (part.Length > inputLength - start)
                        return;

                    success = true;
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    var inpIndex = start;
                    for (var j = 0; j < part.Length; j++)
                    {
                        if (!CheckSymbol(input, part[j], ref inpIndex))
                        {
                            success = false;
                            start++;
                            break;
                        }
                        inpIndex++;
                    }
                    if (success)
                        end = start + part.Length - 1;
                    else end = start;
                } while (!success);

                if (i > 0)
                {
                    var parameterValue = input.ToString(lastEnd + 1, start - lastEnd - 1);
                    constructionLength += parameterValue.Length;
                    implementation = implementation.Replace($"%{parameters[i - 1].Name}%", parameterValue);
                }
                lastEnd = end;
            }
            input.Remove(constructionStart, constructionLength).Insert(constructionStart, implementation);
        }

        private static List<string> SplitConstruction(ConstructionInfo constructionInfo)
        {
            var @interface = FormatInterface(constructionInfo.Interface);
            var parameters = constructionInfo.Parameters;
            var paramsMatches =
                new Regex($@"%({string.Join("|", parameters.Select(x => x.Name))})%").Matches(@interface);

            var parts = new List<string>();
            var lastIndex = 0;
            foreach (Match match in paramsMatches)
            {
                var matchIndex = match.Index;

                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (matchIndex == lastIndex)
                    parts.Add(string.Empty);
                else
                    parts.Add(@interface.Substring(lastIndex, matchIndex - lastIndex));
                lastIndex = matchIndex + match.Length;
            }
            parts.Add(@interface.Substring(lastIndex));
            return parts;
        }

        private static string FormatInterface(string input)
        {
            return Regex.Replace(input.Trim(), @"\s+", " ", RegexOptions.Singleline);
        }
    }
}