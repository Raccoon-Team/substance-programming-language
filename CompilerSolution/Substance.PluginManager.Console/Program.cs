using Substance.PluginManager.Backend;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new PluginExtension(new Kek());
            var config = new Configuration();
            var extensionManager = new ExtensionManager(config);
            foreach (var ext in extensionManager.PluginsExtension)
            {
                System.Console.WriteLine(ext.ToString());
                System.Console.WriteLine();
            }
        }
    }
}