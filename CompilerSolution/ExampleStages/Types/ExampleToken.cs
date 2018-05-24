using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Types
{
    public class ExampleToken : IToken
    {
        public ExampleToken(string value, string type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }
        public string Type { get; }
    }
}