using System;

namespace CompilerUtilities.PluginImporter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = "-input_file test.txt -output_file outp.txt -constructionFiles construction1.xml construction2.xml".Split();
            //var manager = new PluginManager(new[] { "-input_file", "test.txt", "-output_file", "outp.txt", "-" });
            var manager = new PluginManager(args);
            manager.Compile();
            Console.WriteLine("Build successfull!");
            Console.ReadKey();
        }
    }
}