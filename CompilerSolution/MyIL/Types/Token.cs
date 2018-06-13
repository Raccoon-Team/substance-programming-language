using System.Diagnostics;

namespace IL2MSIL
{
    [DebuggerDisplay("{Value} : {TokenType}")]
    public class Token
    {
        public TokenType TokenType;
        public string Value;
        public int Line;

        public Token(TokenType tokenType, string value, int line)
        {
            TokenType = tokenType;
            Value = value;
            Line = line;
        }
    }
}