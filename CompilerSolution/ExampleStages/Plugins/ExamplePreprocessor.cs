using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Plugins
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IPlugin<>))]
    internal class ExamplePreprocessor : IPlugin<ITextProcessor>
    {
        public uint Priority { get; }

        public ITextProcessor Activate(ITextProcessor input)
        {
            CutComments(input);
            ReplaceDefinitions(input);
            return input;
        }

        public void Initialize(IFileBuffer fileBuffer)
        {

        }

        private static void CutComments(ITextProcessor input)
        {
            //var commentReg = new Regex(@"(\\\\|\\\*|\*\\)", RegexOptions.Compiled);

            var multiLineIndex = false;

            var presentation = input.Presentation.ToList();
            for (var i = 0; i < presentation.Count; i++)
            {
                var line = presentation[i];

                //var match = commentReg.Match(line);
                //if (match.Success)
                //{
                    
                //}

                var lineLength = line.Length;
                for (var j = 0; j < lineLength; j++)
                {
                    if (line[j] != '\\')
                        continue;

                    if (!multiLineIndex)
                    {
                        if (j + 1 >= lineLength)
                            continue;

                        if (line[j + 1] == '\\')
                        {
                            presentation[i] = line.Substring(0, j);
                        }
                        else if (line[j + 1] == '*')
                        {
                            presentation[i] = line.Substring(0, j);
                            multiLineIndex = true;
                        }
                    }
                    else
                    {
                        if (j != 0 && line[j - 1] == '*')
                        {
                            multiLineIndex = false;
                            if (j + 1 < lineLength)
                                presentation[i] = line.Substring(j + 1);
                            else presentation[i] = string.Empty;
                        }
                        else
                        {
                            presentation[i] = string.Empty;
                        }
                    }
                }
            }
        }

        private static void ReplaceDefinitions(ITextProcessor input)
        {
            var presentation = input.Presentation.ToList();
            var presentationCount = presentation.Count;

            var defReg = new Regex(@"#define\s+(\w+)(\(.*?\))?\s+(.*)", RegexOptions.Compiled);

            for (var i = 0; i < presentationCount; i++)
            {
                var line = presentation[i];
                var match = defReg.Match(line);

                if (!match.Success) continue;

                presentation.RemoveAt(i);
                i--;
                presentationCount--;

                //var def = new Define(match);
                //def.Replace(presentation);
                ReplaceDef(match, presentation);
            }

            input.Presentation = presentation;
        }

        private static void ReplaceDef(Match match, IList<string> presentation)
        {
            var splitter = new[] {' ', ','};
            var defParameters = match.Groups[2].Value.Trim('(', ')')
                .Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            var replacement = match.Groups[3].Value;
            //if (match.Groups[2].Value != string.Empty)
            var regex = new Regex($@"{match.Groups[1]}\((.*?)\)", RegexOptions.Compiled);

            var presentationCount = presentation.Count;
            for (var i = 0; i < presentationCount; i++)
            {
                var m = regex.Match(presentation[i]);
                if (!m.Success) continue;

                var parameters = m.Groups[1].Value
                    .Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                var parametersLength = parameters.Length;
                for (var j = 0; j < parametersLength; j++)
                    replacement = Regex.Replace(replacement, $@"\b{defParameters[j]}\b", parameters[j]);

                presentation[i] = replacement; // regex.Replace(line, replacement);
            }
        }

        /*private class Define
        {
            private readonly string[] _parameters;
            private readonly Regex _regex;
            private readonly string _replacement;

            public Define(Match match)
            {
                _parameters = match.Groups[2].Value.Trim('(', ')')
                    .Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);

                _replacement = match.Groups[3].Value;
                if (match.Groups[2].Value != string.Empty)
                    _regex = new Regex($@"{match.Groups[1]}\((.*?)\)", RegexOptions.Compiled);
            }

            public void Replace(List<string> presentation)
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
        }*/
    }
}