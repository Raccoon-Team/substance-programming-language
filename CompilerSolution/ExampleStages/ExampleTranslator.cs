using System;
using System.ComponentModel.Composition;
using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.BaseTypes.Interfaces;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;

namespace ExampleStages
{
    [Export(typeof(IStage<,>))]
    public class ExampleTranslator : IStage<ITextProcessor, Blanket>
    {
        public uint Priority { get; }

        [Parameter("-output_file")]
        private string outputFile;

        public void Initialize()
        {
        }

        public Blanket Process(ITextProcessor input)
        {
            File.WriteAllLines(outputFile, input.Presentation);

            return null;
        }
        
    }
}