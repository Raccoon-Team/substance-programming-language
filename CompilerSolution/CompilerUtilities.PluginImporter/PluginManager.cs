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
using CompilerUtilities.Plugins.Contract.Interfaces;
using CompilerUtilities.Plugins.Contract.Versions;

namespace CompilerUtilities.PluginImporter
{
    public class PluginManager
    {
        [ImportMany(typeof(IPlugin<>))] private List<object> _plugins;
        [ImportMany(typeof(IStage<,>))] private List<object> _stages;

        public PluginManager(string[] args)
        {
            var cat = new AggregateCatalog();
            cat.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory() + "\\plugins"));
            cat.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory() + "\\stages"));
            var container = new CompositionContainer(cat);
            container.ComposeParts(this);

            ConsoleParameters.Initialize(_plugins.Concat(_stages).Prepend(this).ToArray(), args);

            ValidateExtensions();

            CheckForDuplicateStages();
        }

        [Parameter("-help", true)]
        private static void ShowAvailableParameters()
        {
            var available = ConsoleParameters.GetAllAvailableParameters();
            for (var i = 0; i < available.Count; i++)
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

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < allExtensions.Count; i++)
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

        private static void Compile(IEnumerable<object> sequence)
        {
            object param = new Blanket();
            var emptyParams = new object[0];

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in sequence)
            {
                item.GetType().GetMethod("Initialize").Invoke(item, emptyParams);
                param = item.GetType().GetMethod("Process").Invoke(item, new[] {param});
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

        public List<object> ComposeStages(List<object> input, bool throwException = true)
        {
            if (input.Count == 1)
                return input;
            var sortedInput = new List<object>(input.OrderBy(o =>
            {
                var (TIn, TOut) = PluginGenericArgs.GetArgs(o);
                if (TIn == typeof(Blanket)) return 0;
                return TOut == typeof(Blanket) ? 2 : 1;
            }));

            var outp = new List<object>();

            var entry = sortedInput.First(o => PluginGenericArgs.GetArgs(o).TIn == typeof(Blanket));
            sortedInput.Remove(entry);
            outp.Add(entry);

            while (sortedInput.Count > 0)
            {
                var current =
                    sortedInput.FirstOrDefault(o =>
                        PluginGenericArgs.GetArgs(o).TIn == PluginGenericArgs.GetArgs(outp.Last()).TOut);

                if (current is null)
                    if (throwException)
                        throw new StagesCompositionException();
                    else return outp;
                outp.Add(current);
                sortedInput.Remove(current);
            }

            return outp;
        }
    }
}