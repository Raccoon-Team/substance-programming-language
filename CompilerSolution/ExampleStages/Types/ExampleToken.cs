using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Types
{
    public class ExampleToken : IToken
    {
        public ExampleToken(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }
        public TokenType Type { get; }
    }
}