using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CompilerUtilities.Exceptions;

namespace IL2MSIL
{
    // ReSharper disable once InconsistentNaming
    internal static class ILTokenizer
    {
        private static readonly string[] constructions = {"func", "while", "if", "ret"};
        private static string[] typeDef = {"class", "struct", "interface"};
        private static readonly char[] operators = {'=', '*', '/', '+', '-', '~', '&', '|', '^', '!', '>', '<'};

        private static readonly List<string> modifiers = new List<string>
        {
            "private",
            "public",
            "protected",
            "internal",
            "static",
            "sealed",
            "virtual",
            "abstract"
        };

        public static List<Token> Tokenize(IList<string> lines, IList<string> baseTypes, out IList<(string, TypeAttributes)> customTypes)
        {
            customTypes = ParseTypes(lines);

            var types = baseTypes.Concat(customTypes.Select(x => x.Item1)).ToList();

            var accum = new StringBuilder();
            var tokens = new List<Token>();
            var quote = false;

            var linesCount = lines.Count;
            for (var i = 0; i < linesCount; i++)
            {
                var line = lines[i];
                var lineLength = line.Length;
                for (var j = 0; j < lineLength; j++)
                {
                    var chr = line[j];

                    var isSplitter = char.IsWhiteSpace(chr) || chr == ',' || chr == '.';

                    bool? isBraces = null;
                    // ReSharper disable once ConvertIfStatementToSwitchStatement
                    if (chr == '(')
                        isBraces = true;
                    else if (chr == ')')
                        isBraces = false;

                    if (!quote && (isSplitter || isBraces != null))
                    {
                        FlushAccum(accum, tokens, types, i);
                        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                        if (isBraces == true)
                            tokens.Add(new Token(TokenType.OpenBrace, chr.ToString(), i));
                        else if (isBraces == false)
                            tokens.Add(new Token(TokenType.CloseBrace, chr.ToString(), i));
                        continue;
                    }

                    if (chr == '"')
                        quote = !quote;

                    accum.Append(chr);
                }
                FlushAccum(accum, tokens, types, i);
            }

            return tokens;
        }

        private static void FlushAccum(StringBuilder accum, ICollection<Token> tokens, IList<string> types, int lineIndex)
        {
            if (accum.Length == 0)
                return;

            tokens.Add(ParseToken(accum, types, lineIndex));
            accum.Clear();
        }

        private static IList<(string, TypeAttributes)> ParseTypes(IList<string> lines)
        {
            var types = new List<(string, TypeAttributes)>();

            var linesCount = lines.Count;
            for (var i = 0; i < linesCount; i++)
            {
                var line = lines[i];
                var match = Regex.Match(line,
                    @"(private|public|protected|internal|static|sealed|abstract|\s+)*\s*class\s+([^\s]+)",
                    RegexOptions.Compiled);


                if (!match.Success) continue;

                var typeAttributes = match.Groups[1].Value.Split().Select(s =>
                {
                    if (s == string.Empty)
                        return (TypeAttributes)0;
                    if (Enum.TryParse(s, true, out TypeAttributes atr))
                        return atr;
                    ExceptionManager.ThrowCompiler(ErrorCode.ModifierExpected, "", i);
                    return (TypeAttributes) 0;
                }).Aggregate((first, second) => first | second);

                types.Add((match.Groups[2].Value, typeAttributes));
            }

            return types;
        }

        private static Token ParseToken(StringBuilder value, IList<string> types, int lineIndex)
        {
            TokenType type;

            var strValue = value.ToString();

            if (value[0] == '"' || char.IsDigit(value[0]))
                type = TokenType.Constant;
            else if (operators.Contains(value[0]))
                type = TokenType.Operator;
            else if (constructions.Contains(strValue))
                type = TokenType.Construction;
            else if (types.Contains(strValue))
                type = TokenType.Type;
            else if (modifiers.Contains(strValue))
                type = TokenType.Modifier;
            else if (typeDef.Contains(strValue))
                type = TokenType.TypeDef;
            else if (strValue == "end")
                return new Token(TokenType.End, string.Empty, lineIndex);
            else type = TokenType.Identifier;

            return new Token(type, strValue, lineIndex);
        }
    }
}