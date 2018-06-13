using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleConstructionsParser : IStage<Blanket, IList<ConstructionInfo>>
    {
        [Parameter("-constructionFiles")] private string[] constructionFiles;

        private IFileBuffer _fileBuffer;

        public void Initialize(IFileBuffer fileBuffer)
        {
            _fileBuffer = fileBuffer;
        }

        public IList<ConstructionInfo> Process(Blanket input)
        {
            var constructions = new List<ConstructionInfo>();

            for (var i = 0; i < constructionFiles.Length; i++)
                constructions.Add(ConstructionInfo.ParseFromFile(constructionFiles[i]));

            return constructions;
        }
    }
}