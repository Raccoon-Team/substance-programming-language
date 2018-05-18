using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Substance.PluginManager.Backend;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
