using System.Linq;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Types
{
    internal class TokenTypesCollection
    {
        private string[] _keywords, _operators, _semicolons, _specials;

        public TokenType this[string value]
        {
            get
            {
                if (_keywords.Contains(value))
                    return TokenType.Keyword;
                if (_operators.Contains(value))
                    return TokenType.Operator;
                if (_semicolons.Contains(value))
                    return TokenType.Semicolon;
                if (value[0] == '"' && value[value.Length - 1] == '"')
                    return TokenType.String;
                if (_specials.Contains(value))
                    return TokenType.Special;

                return TokenType.Identifier;
            }
        }

        public void Initialize()
        {
            _keywords = new[] {"mov", "add", "sub", "div", "mul", "ax", "bx"};
            _semicolons = new[] {","};
            _operators = new string[0];
            _specials = new string[0];
        }

        public bool CompareCharCategory(char first, char second)
        {
            bool isLetter(char ch)
            {
                return char.IsLetter(ch) || ch == '_';
            }

            if (isLetter(first) && isLetter(second))
                return true;

            return false;
        }
    }
}