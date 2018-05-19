using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Interfaces;

namespace Substance.PluginManager.Backend
{
    public class StageBox
    {
        [ImportMany(typeof(IStage<,>))] public List<object> Stages;

        public StageBox(string path)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetFullPath(path)));
            var compositionContainer = new CompositionContainer(catalog);
            compositionContainer.ComposeParts(this);
        }
    }
}