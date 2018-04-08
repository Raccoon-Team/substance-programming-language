using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.Stages
{
    public interface IGeneratorIntermediate
    {
        ITextProcessor Generate(ISyntaxTree tree);
    }
}