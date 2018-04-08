using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.Stages
{
    public interface IFileReader
    {
        ITextProcessor ReadFromFile(string path);
    }
}