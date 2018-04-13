using System;

namespace CompilerUtilities.PluginImporter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = new PluginManager(new []{"input_file","test.txt","output_file","outp.txt"});
            manager.Compile();
            Console.ReadKey();
        }
    }
}