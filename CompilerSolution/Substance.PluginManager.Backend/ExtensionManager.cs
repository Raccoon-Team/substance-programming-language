using System.Collections.Generic;
using System.Linq;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Backend
{
    public class ExtensionManager
    {
        private StageBox _stages;
        private PluginBox _plugins;

        public List<IExtension> StagesExtension;
        public List<IExtension> PluginsExtension;

        public ExtensionManager(Configuration globalConfig)
        {
            if(globalConfig.IsDefined("StagePath"))
                _stages = new StageBox(globalConfig["StagePath"].Value);
            if(globalConfig.IsDefined("PluginPath"))
                _plugins = new PluginBox(globalConfig["PluginPath"].Value);

            StagesExtension = new List<IExtension>();
            PluginsExtension = new List<IExtension>();

            ParsePlugins();
        }

        private void ParsePlugins()
        {
            PluginsExtension = _plugins.Plugins.Select(elem =>(IExtension) new PluginExtension(elem)).ToList();
        }
    }
}