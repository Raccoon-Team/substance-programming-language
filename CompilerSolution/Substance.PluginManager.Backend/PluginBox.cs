using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using CompilerUtilities.Plugins.Contract;

namespace Substance.PluginManager.Backend
{
    public class PluginBox
    {
        [ImportMany(typeof(IPlugin<>))] public List<object> Plugins;

        public PluginBox(string path)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetFullPath(path)));
            var compositionContainer = new CompositionContainer(catalog);
            compositionContainer.ComposeParts(this);
        }
    }
}