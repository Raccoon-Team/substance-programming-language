using System;
using System.IO;
using CompilerUtilities.Plugins.Contract;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Backend
{
    public class PluginExtension : IExtension
    {
        public PluginExtension(object plugin)
        {

            var interfaces = plugin.GetType().FindInterfaces((type, criteria) 
                => typeof(IPlugin<>).Name == type.Name && 
                   typeof(IPlugin<>).Namespace == type.Namespace && 
                   typeof(IPlugin<>).Assembly == type.Assembly, null);

            if (interfaces.Length != 1)
                throw new ArgumentException("Invalid object type");

            var location = Path.GetDirectoryName(plugin.GetType().Assembly.Location).ToLower();
            var iplugin = interfaces[0];
            var types = iplugin.GenericTypeArguments;

            Types = types;
            ExtensionFolder = location;
            Info = new ExtensionInfo("Title", "Description", "Author");
            Configuration = new Configuration(Path.Combine(location, "config.xaml"));
        }

        public Configuration Configuration { get; }
        public ExtensionInfo Info { get; }
        public string ExtensionFolder { get; }
        public ExtensionStatus Status { get; }
        public Type[] Types { get; }

        public override string ToString()
        {
            return $"{nameof(Info)}:\n{Info}\n" +
                   //$"{nameof(Configuration)}: {Configuration}\n" +
                   $"{nameof(ExtensionFolder)}: {ExtensionFolder}\n" +
                   $"{nameof(Status)}: {Status}\n" +
                   $"Type: {Types[0]}";
        }
    }
}