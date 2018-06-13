using System.Diagnostics;

namespace IL2MSIL
{
    [DebuggerDisplay("{Value} : {TokenType}")]
    public class Token
    {
        public TokenType TokenType;
        public string Value;

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }
    }
}