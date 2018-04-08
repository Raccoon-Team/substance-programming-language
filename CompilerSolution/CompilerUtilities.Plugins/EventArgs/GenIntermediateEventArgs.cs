using CompilerUtilities.BaseTypes.Interfaces;

namespace CompilerUtilities.Plugins.EventArgs
{
    public class GenIntermediateEventArgs:FileReaderEventArgs
    {
        public GenIntermediateEventArgs(ITextProcessor textProcessor) : base(textProcessor)
        {
        }
    }
}