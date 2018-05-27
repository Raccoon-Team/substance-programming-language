using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdvancedConsoleParameters;
using CompilerUtilities.Plugins.Contract;
using ExampleStages.Types;

namespace ExampleStages.Stages
{
    [RequiredCompilerVersion("a0.1")]
    [Export(typeof(IStage<,>))]
    public class ExampleConstructionsInliner : IStage<IList<ConstructionInfo>, ITextProcessor>
    {
        [Parameter("-input_file")] private string sourceFileName;

        public ITextProcessor Process(IList<ConstructionInfo> input)
        {
            var src = File.ReadAllText(sourceFileName);

            var spacesReg = new Regex(@"\s+");

            //src = spacesReg.Replace(src, " ");

            foreach (var constructionInfo in input)
            {
                var @interface = constructionInfo.Interface.Trim();
                @interface = Regex.Replace(@interface, @"([()\.?\\])", @"\$1");
                @interface = spacesReg.Replace(@interface, "[\\s\r\n]+");

                var implementation = constructionInfo.Implementation.Trim();
                //implementation = spacesReg.Replace(implementation, " ");

                var parameters = constructionInfo.Parameters.Select(x => x.Name).ToArray();

                foreach (var parameter in parameters)
                    @interface = @interface.Replace($"%{parameter}%", $@"(?<{parameter}>.+?)");

                var reg = new Regex(@interface, RegexOptions.Multiline);
                var matches = reg.Matches(src);
                foreach (Match match in matches)
                {
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        implementation = implementation.Replace($"%{parameter}%", match.Groups[parameter].Value);
                    }
                    src = reg.Replace(src, implementation);
                }
            }

            return new ExampleTextProcessor(new []{src});
        }

        public void Initialize(IFileBuffer fileBuffer)
        {

        }
    }
}