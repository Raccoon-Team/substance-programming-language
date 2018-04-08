using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
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

        private static void Compile(IReadOnlyList<object> sequence)
        {
            object param = new Blanket();
            for (var i = 0; i < sequence.Count; i++)
                param = sequence[i].GetType().GetMethod("DoStage").Invoke(sequence[i], new[] {param});
        }

        public void CheckForDuplicate()
        {
            var converted = _stages.Select(x => new PluginGenericArgs(x));

            if (converted.All(x => x.TIn != typeof(Blanket)) || converted.All(x => x.TOut != typeof(Blanket)))
                throw new Exception("Не обнаружены начальная или конечная стадии");

            if (converted.Any(stage => converted.Count(x => x.TIn == stage.TIn && x.TOut == stage.TOut) > 1))
                throw new Exception("Обнаружены одинаковые стадии");
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
                        throw new NotImplementedException();
                    else return outp;
                outp.Add(current);
                copy.Remove(current);
            }

            return outp;
        }
    }
}