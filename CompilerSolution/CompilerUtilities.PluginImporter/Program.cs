using System;

namespace CompilerUtilities.PluginImporter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = new PluginManager();
            manager.Compile();
            Console.ReadKey();
        }
    }
}