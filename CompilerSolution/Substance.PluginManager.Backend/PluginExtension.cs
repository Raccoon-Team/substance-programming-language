using System;
using System.Diagnostics;
using System.IO;
using CompilerUtilities.Plugins.Contract;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Backend
{
    public class PluginExtension : IExtension
    {
        public PluginExtension(object plugin)
        {
            var interfaces = plugin.GetType().FindInterfaces((type, criteria) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IPlugin<>), null);

            if (interfaces.Length != 1)
                throw new ArgumentException("Invalid object type");

            var location = Path.GetDirectoryName(plugin.GetType().Assembly.Location).ToLower();
            var versionInfo = FileVersionInfo.GetVersionInfo(location);
            var iplugin = interfaces[0];
            var types = iplugin.GenericTypeArguments;

            Types = types;
            ExtensionFolder = location;
            Info = new ExtensionInfo(versionInfo.ProductName, versionInfo.FileDescription, versionInfo.CompanyName);
            Configuration = new Configuration(Path.Combine(location, "config.xaml"));
        }

        public Configuration Configuration { get; }
        public ExtensionInfo Info { get; }
        public string ExtensionFolder { get; }
        public ExtensionStatus Status { get; }
        public Type[] Types { get; }

        public override string ToString()
        {
            return $"{nameof(Info)}:\n\r{Info}\n\r" +
                   //$"{nameof(Configuration)}: {Configuration}\n\rs" +
                   $"{nameof(ExtensionFolder)}: {ExtensionFolder}\n\r" +
                   $"{nameof(Status)}: {Status}\n\r" +
                   $"Type: {Types[0]}";
        }
    }
}