﻿using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Stages.Old
{
    [RequiredCompilerVersion("a0.1")]
    //[Export(typeof(IStage<,>))]
    public class ExampleTranslator : IStage<ITextProcessor, Blanket>
    {
        [Parameter("-output_file")] private string _outputFile;

        public uint Priority { get; }

        public void Initialize(IFileBuffer fileBuffer)
        {
        }

        public Blanket Process(ITextProcessor input)
        {
            File.WriteAllLines(_outputFile, input.Presentation);

            return null;
        }
    }
}