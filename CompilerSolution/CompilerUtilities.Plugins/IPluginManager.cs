using System;
using CompilerUtilities.Plugins.EventArgs;

namespace CompilerUtilities.Plugins
{
    public interface IPluginManager
    {
        EventHandler<FileReaderEventArgs> OnFileReadEnd { get; set; }
        EventHandler<LexerEventArgs> OnTokenized { get; set; }
        EventHandler<ParserEventArgs> OnParsed { get; set; }
        EventHandler<GenIntermediateEventArgs> OnIntermediateCodeGenerated { get; set; }
        EventHandler OnTranslated { get; set; }

        void Notify(string message);
    }
}