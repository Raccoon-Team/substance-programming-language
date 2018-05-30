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

        public ITextProcessor Process(IList<ConstructionInfo> input)
        {
            var src = File.ReadAllText(sourceFileName);

            //todo причесать код, он ужасен
            //todo оптимизировать код, он ужасен
            //todo регексов много не бывает

            foreach (var constructionInfo in input)
            {
                var @interface = constructionInfo.Interface.Trim();
                var implementation = constructionInfo.Implementation.Trim();

                var parts = new Regex(@"(.*?)(%[^\s]+?%)", RegexOptions.Singleline).Matches(@interface).Cast<Match>()
                    .Select(x => (part: x.Groups[1].Value, parameter: x.Groups[2].Value)).ToList();

                Match afterParamMatch = null;

                for (var i = 0; i < parts.Count - 1; i++)
                {
                    var currentPart = PreparePart(parts[i]);
                    var nextPart = PreparePart(parts[i + 1]);

                    var beforeParamMatch = Regex.Match(src, currentPart.part);
                    var paramStart = beforeParamMatch.Index + beforeParamMatch.Length;

                    afterParamMatch = Regex.Match(src, nextPart.part);
                    var paramEnd = afterParamMatch.Index;

                    var replacement = src.Substring(paramStart, paramEnd - paramStart);
                    implementation = implementation.Replace(currentPart.parameter, replacement);
                    @interface = @interface.Replace(currentPart.parameter, replacement);
                }

                var lastParamMatch = Regex.Match(@interface, parts.Last().parameter, RegexOptions.Singleline);
                var lastPart = @interface.Substring(lastParamMatch.Index + lastParamMatch.Length + 1);
                lastPart = PreparePart(lastPart);

                var startIndex = afterParamMatch.Index + afterParamMatch.Length;
                var paramMatch = Regex.Matches(src, lastPart).Cast<Match>().First(x => x.Index > startIndex);

                var paramValue = src.Substring(startIndex, paramMatch.Index - startIndex);
                implementation = implementation.Replace(parts.Last().parameter, paramValue);
                @interface = @interface.Replace(parts.Last().parameter, paramValue);

                @interface = PreparePart(@interface);
                src = Regex.Replace(src, @interface, implementation);
            }

            return new ExampleTextProcessor(new[] {src});
        }

        static (string part, string parameter) PreparePart((string part, string parameter) part)
        {
            part.part = PreparePart(part.part);
            return part;
        }

        static string PreparePart(string part)
        {
            part = Regex.Replace(part, @"\s{2,}", "\\s+", RegexOptions.Compiled);
            part = Regex.Replace(part, @"([*()\[\]])", "\\$1", RegexOptions.Compiled);
            return part;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {
        }

        private static int FindPartIndex(int startIndex, string src, string part)
        {
            var partLength = part.Length;
            var srcLength = src.Length;

            while (true)
            {
                while (src[startIndex] != part[0])
                {
                    startIndex++;
                    if (startIndex == srcLength)
                        return -1;
                }

                for (var j = 1; j < partLength; j++)
                {
                    if (src[startIndex + j] != part[j])
                    {
                        startIndex++;
                        break;
                    }

                    if (j + 1 == partLength)
                        return startIndex;
                }
            }
        }

        private int FindClosingBraceIndex(string src, int startIndex)
        {
            var srcLength = src.Length;
            var difference = 0;
            for (var i = startIndex; i < srcLength; i++)
            {
                if (src[i] == ')')
                    difference++;
                else if (src[i] == '(')
                    difference--;

                if (difference == 0) return i;
            }

            return srcLength - 1;
        }
    }
}