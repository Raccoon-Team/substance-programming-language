using System;
using CompilerUtilities.Plugins.Contract;
using CompilerUtilities.Plugins.Contract.Versions;
using Substance.PluginManager.Backend;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Console
{
    [RequiredCompilerVersion("12.5a")]
    internal class Kek : IPlugin<int>, IPlugin<bool>
    {
        bool IPlugin<bool>.Activate(bool input)
        {
            throw new NotImplementedException();
        }

        public int Activate(int input)
        {
            return 0;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var kek = new Kek();
            var intMethod = kek.GetType().GetMethod("Activate");
            intMethod.Invoke(kek, new object[] {45});

            var boolMethod = kek.GetType().GetMethod("Activate");
            boolMethod.Invoke(kek, new object[] {true});

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