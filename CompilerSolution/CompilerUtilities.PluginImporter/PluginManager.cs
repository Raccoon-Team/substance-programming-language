using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using AdvancedConsoleParameters;
using CompilerUtilities.Exceptions;
using CompilerUtilities.Plugins.Contract;

namespace CompilerUtilities.PluginImporter
{
    public class PluginManager
    {
        [ImportMany(typeof(IPlugin<>))] private List<ICompilerExtension> _plugins;
        [ImportMany(typeof(IStage<,>))] private List<ICompilerExtension> _stages;

        public PluginManager(string[] args)
        {
            var catalog = new AggregateCatalog();
            var currentDirectory = Directory.GetCurrentDirectory();
            catalog.Catalogs.Add(new DirectoryCatalog(currentDirectory + "\\plugins"));
            catalog.Catalogs.Add(new DirectoryCatalog(currentDirectory + "\\stages"));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            ConsoleParameters.Initialize(_plugins.Cast<object>().Concat(_stages).Prepend(this).ToArray(), args);

            ValidateExtensions();

            CheckForDuplicateStages();
        }

        [Parameter("-help", true)]
        private void ShowAvailableParameters()
        {
            var available = ConsoleParameters.GetAllAvailableParameters(_plugins.Cast<object>().Concat(_stages).Prepend(this).ToArray());
            var availableCount = available.Count;
            for (var i = 0; i < availableCount; i++)
            {
                var current = available[i];

                Console.WriteLine(current);
                Console.WriteLine();
            }
            Console.ReadKey();
            Environment.Exit(0);
        }

        private void ValidateExtensions()
        {
            var allExtensions = _stages.Concat(_plugins).ToList();
            var compilerVersion = Assembly.GetExecutingAssembly().GetName().Version;

            var allExtensionsCount = allExtensions.Count;
            for (var i = 0; i < allExtensionsCount; i++)
            {
                var ext = allExtensions[i];

                var attribute = ext.GetType().GetCustomAttribute<RequiredCompilerVersionAttribute>();
                if (attribute == null)
                    throw new RequiredAttributeNotFoundException(
                        "One of the plugins/stages does not have an attribute \"RequiredCompilerVersionAttribute\"",
                        ext, typeof(RequiredCompilerVersionAttribute));

                var extVersion = attribute.GetRequiredVersion;
                if (extVersion > compilerVersion)
                    throw new ExtensionNotCompatibleException(
                        "The version of this extension is not compatible with the current version of the compiler", ext,
                        extVersion);
            }
        }

        public void Compile()
        {
            var chain = ComposeStages(_stages, _plugins, false);
            Compile(chain);
        }

        private static void Compile(IEnumerable<ICompilerExtension> sequence)
        {
            object param = new Blanket();
            var emptyParams = new object[0];

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in sequence)
            {
                var itemType = item.GetType();

                var isStage =
                    itemType.FindInterfaces(
                        (type, criteria) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IStage<,>),
                        null).Length > 0;
                if (isStage)
                {
                    itemType.GetMethod("Initialize").Invoke(item, emptyParams);
                    param = itemType.GetMethod("Process").Invoke(item, new[] {param});
                }
                else
                {
                    param = itemType.GetMethod("Activate").Invoke(item, new[] { param });
                }
            }
        }

        public void CheckForDuplicateStages()
        {
            var converted = _stages.Select(x => new PluginGenericArgs(x)).ToList();

            if (converted.All(x => x.TIn != typeof(Blanket)))
                throw new StageNotFoundException("Start stage not found");
            if (converted.All(x => x.TOut != typeof(Blanket)))
                throw new StageNotFoundException("End stage not found");

            var сonvertedCount = converted.Count;
            for (var i = 0; i < сonvertedCount; i++)
            for (var j = i + 1; j < сonvertedCount; j++)
                if (converted[i] == converted[j])
                    throw new DuplicatedStagesException(
                        $"The same stages are found with the parameters <{converted[i].TIn.Name}, {converted[i].TOut.Name}>");
        }

        public List<ICompilerExtension> ComposeStages(List<ICompilerExtension> stages, List<ICompilerExtension> plugins, bool throwException = true)
        {
            if (stages.Count == 1)
                return stages;
            var sortedStages = new List<ICompilerExtension>(stages.OrderBy(o =>
            {
                var args = PluginGenericArgs.GetArgs(o);
                if (args[0] == typeof(Blanket)) return 0;
                return args[1] == typeof(Blanket) ? 2 : 1;
            }));

            var outp = new List<ICompilerExtension>();

            var entry = sortedStages.FindIndex(o => PluginGenericArgs.GetArgs(o)[0] == typeof(Blanket));
            outp.Add(sortedStages[entry]);
            sortedStages.RemoveAt(entry);

            var sortedInputCount = sortedStages.Count;

            while (sortedInputCount > 0)
            {
                var pluginIndex = plugins.FindIndex(p =>
                    PluginGenericArgs.GetArgs(p)[0] == PluginGenericArgs.GetArgs(outp.Last()).Last());

                if (pluginIndex != -1)
                {
                    outp.Add(plugins[pluginIndex]);
                    plugins.RemoveAt(pluginIndex);
                }

                var currentStage =
                    sortedStages.FindIndex(o =>
                    {
                        var oIn = PluginGenericArgs.GetArgs(o)[0];
                        var outpOut = PluginGenericArgs.GetArgs(outp.Last()).Last();
                        return oIn == outpOut;
                    });

                if (currentStage == -1)
                    if (throwException)
                        throw new StagesCompositionException();
                    else return outp;
                outp.Add(sortedStages[currentStage]);
                sortedStages.RemoveAt(currentStage);
                sortedInputCount--;
            }

            return outp;
        }
    }
}