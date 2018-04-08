using System;
using System.Reflection;

namespace GenericImportingDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //if (args is null) Console.WriteLine("Null args");
            //else if (args.Length == 0) Console.WriteLine("Empty args");
            //else
            //    for (var i = 0; i < args.Length; i++)
            //        Console.WriteLine(args[i]);
            //return;
            var manager = new PluginManager();
            //manager.Compile();
            var asm = Assembly.GetExecutingAssembly();
            var refs = asm.GetReferencedAssemblies();
            manager.Compile();
            Console.ReadKey();
        }
    }
}