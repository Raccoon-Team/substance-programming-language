using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.Stages
{
    public interface ITranslator
    {
        void Translate(ITextProcessor intermediateCode, string outputPath);
    }
}