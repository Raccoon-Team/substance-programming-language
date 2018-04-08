using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using CompilerUtilities.Exceptions;
using CompilerUtilities.PluginContract;

namespace CompilerUtilities.PluginImporter
{
    internal class PluginManager
    {
        [ImportMany(typeof(IStage<,>))] private List<object> _stages;

        public PluginManager()
        {
            var cat = new AggregateCatalog();
            cat.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory() + "\\plugins"));
            var container = new CompositionContainer(cat);
            container.ComposeParts(this);

            CheckForDuplicate();
        }

        public void Compile()
        {
            var chain = ComposeChain(_stages, false);
            Compile(chain);
        }

        private static void Compile(IEnumerable<object> sequence)
        {
            object param = new Blanket();
            foreach (var item in sequence)
                param = item.GetType().GetMethod("DoStage").Invoke(item, new[] {param});
        }

        public void CheckForDuplicate()
        {
            var converted = _stages.Select(x => new PluginGenericArgs(x)).ToList();

            if (converted.All(x => x.TIn != typeof(Blanket)))
                throw new StageNotFoundException("Start stage not found");
            if (converted.All(x => x.TOut != typeof(Blanket)))
                throw new StageNotFoundException("End stage not found");

            for (var i = 0; i < converted.Count; i++)
            {
                for (var j = i+1; j < converted.Count; j++)
                {
                    if (converted[i].TIn == converted[j].TIn && converted[i].TOut == converted[j].TOut)
                        throw new DuplicatedStagesException($"The same stages are found with the parameters <{converted[i].TIn.Name}, {converted[i].TOut.Name}>");
                }
            }
        }

        public void GetInfo()
        {
            foreach (var stage in _stages)
            {
                var @interface = stage.GetType().FindInterfaces((type, criteria) => true, null)[0];

                var generic = @interface.GenericTypeArguments;

                Console.WriteLine(
                    $"{stage.GetType().Name} is IStage<{string.Join(", ", generic.Select(x => x.Name))}>");

                Console.WriteLine();
            }
        }

        public List<object> ComposeChain(List<object> input, bool throwException = true)
        {
            if (input.Count == 1)
                return input;
            var copy = new List<object>(input);

            var outp = new List<object>();

            var entry = copy.First(o => new PluginGenericArgs(o).TIn == typeof(Blanket));
            copy.Remove(entry);
            outp.Add(entry);

            while (copy.Count > 0)
            {
                var current =
                    copy.FirstOrDefault(o => new PluginGenericArgs(o).TIn == new PluginGenericArgs(outp.Last()).TOut);
                if (current is null)
                    if (throwException)
                        throw new StagesCompositionException();
                    else return outp;
                outp.Add(current);
                copy.Remove(current);
            }

            return outp;
        }
    }
}