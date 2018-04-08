using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.EventArgs
{
    public class FileReaderEventArgs:System.EventArgs
    {
        public ITextProcessor TextProcessor;

        public FileReaderEventArgs(ITextProcessor textProcessor)
        {
            TextProcessor = textProcessor;
        }
    }
}