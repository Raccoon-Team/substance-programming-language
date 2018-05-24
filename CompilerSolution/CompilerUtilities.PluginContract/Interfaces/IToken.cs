namespace CompilerUtilities.Plugins.Contract
{
    public interface IToken
    {
        string Value { get; }
        TokenType Type { get; }
    }
}