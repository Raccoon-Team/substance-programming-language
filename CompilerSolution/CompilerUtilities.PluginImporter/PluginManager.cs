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
            var cat = new AggregateCatalog();
            cat.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory() + "\\plugins"));
            cat.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory() + "\\stages"));
            var container = new CompositionContainer(cat);
            container.ComposeParts(this);

            ConsoleParameters.Initialize(_plugins.Cast<object>().Concat(_stages).Prepend(this).ToArray(), args);

            ValidateExtensions();

            CheckForDuplicateStages();
        }

        [Parameter("-help", true)]
        private static void ShowAvailableParameters()
        {
            var available = ConsoleParameters.GetAllAvailableParameters();
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
            var chain = ComposeStages(_stages, false);
            Compile(chain);
        }

        private static void Compile(IEnumerable<ICompilerExtension> sequence)
        {
            object param = new Blanket();
            var emptyParams = new object[0];

            var initializationMethod = typeof(IStage<,>).GetMethod("Initialize");
            var processMethod = typeof(IStage<,>).GetMethod("Process");
            var activateMethod = typeof(IPlugin<>).GetMethod("Activate");

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in sequence)
            {
                var itemType = item.GetType();

                if (itemType.GetGenericTypeDefinition() == typeof(IStage<,>))
                {
                    initializationMethod.Invoke(item, emptyParams);
                    param = processMethod.Invoke(item, new[] {param});
                }
                else
                {
                    param = activateMethod.Invoke(item, new[] { param });
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

        public List<ICompilerExtension> ComposeStages(List<ICompilerExtension> input, bool throwException = true)
        {
            if (input.Count == 1)
                return input;
            var sortedInput = new List<ICompilerExtension>(input.OrderBy(o =>
            {
                var (TIn, TOut) = PluginGenericArgs.GetArgs(o);
                if (TIn == typeof(Blanket)) return 0;
                return TOut == typeof(Blanket) ? 2 : 1;
            }));

            var outp = new List<ICompilerExtension>();

            var entry = sortedInput.FindIndex(o => PluginGenericArgs.GetArgs(o).TIn == typeof(Blanket));
            sortedInput.RemoveAt(entry);
            outp.Add(sortedInput[entry]);

            var sortedInputCount = sortedInput.Count;

            while (sortedInputCount > 0)
            {
                var current =
                    sortedInput.FindIndex(o =>
                        PluginGenericArgs.GetArgs(o).TIn == PluginGenericArgs.GetArgs(outp.Last()).TOut);

                if (current == -1)
                    if (throwException)
                        throw new StagesCompositionException();
                    else return outp;
                outp.Add(sortedInput[current]);
                sortedInput.RemoveAt(current);
                sortedInputCount--;
            }

            return outp;
        }
    }
}