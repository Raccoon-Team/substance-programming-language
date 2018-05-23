using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Interfaces;

namespace ExampleStages.Plugins
{
    internal class ExamplePreprocessor : IPlugin<ITextProcessor>
    {
        public ITextProcessor Activate(ITextProcessor input)
        {
            CutComments(input);
            ReplaceDefinitions(input);
            return input;
        }

        private static void CutComments(ITextProcessor input)
        {
            var presentation = input.Presentation.ToList();

            presentation.RemoveAll(s => s.TrimStart().StartsWith("//"));

            var lineCommentReg = new Regex(@"(.*?)//.*?", RegexOptions.Compiled);

            var presentationCount = presentation.Count;
            for (var i = 0; i < presentationCount; i++)
            {
                var line = presentation[i];

                if (lineCommentReg.IsMatch(line))
                    presentation[i] = lineCommentReg.Replace(line, "$1");
            }

            input.Presentation = presentation;
        }

        private static void ReplaceDefinitions(ITextProcessor input)
        {
            var presentation = input.Presentation.ToList();

            var defReg = new Regex(@"#define\s+(\w+)(\(.*?\))?\s+(.*)", RegexOptions.Compiled);

            for (var i = 0; i < presentation.Count; i++)
            {
                var line = presentation[i];
                var match = defReg.Match(line);

                if (!match.Success) continue;

                var def = new DefineMatch(match);
                def.Process(presentation);
            }

            input.Presentation = presentation;
        }
    }

    internal class DefineMatch
    {
        private readonly string _replacement;
        private readonly Regex _regex;
        private readonly string[] _parameters;

        public DefineMatch(Match match)
        {
            _parameters = match.Groups[2].Value.Trim('(', ')')
                .Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

            _replacement = match.Groups[3].Value;
            if (match.Groups[2].Value != string.Empty)
                _regex = new Regex($@"{match.Groups[1]}\((.*?)\)", RegexOptions.Compiled);
        }

        public void Process(List<string> presentation)
        {
            var splitter = new[] {' ', ','};

            var presentationCount = presentation.Count;
            for (var i = 0; i < presentationCount; i++)
            {
                var line = presentation[i];

                var match = _regex.Match(line);
                if (!match.Success) continue;

                var parameters = match.Groups[1].Value
                    .Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                var replacement = _replacement;

                var parametersLength = parameters.Length;
                for (var j = 0; j < parametersLength; j++)
                {
                    var parameter = parameters[j];
                    replacement = Regex.Replace(replacement, $@"\b{_parameters[j]}\b", parameter);
                }
                presentation[i] = _regex.Replace(line, replacement);
            }
        }
    }
}