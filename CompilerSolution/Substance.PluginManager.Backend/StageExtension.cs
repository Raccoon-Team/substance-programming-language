using System;
using System.IO;
using Substance.PluginManager.Backend.Configs;
using CompilerUtilities.Plugins.Contract.Interfaces;
using System.Diagnostics;

namespace Substance.PluginManager.Backend
{
    class StageExtension : IExtension
    {
        public StageExtension(object stage)
        {
            var interfaces = stage.GetType().FindInterfaces((type, criteria) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IStage<,>), null);

            if (interfaces.Length != 1)
                throw new ArgumentException("Invalid object type");

            
            var location = Path.GetDirectoryName(stage.GetType().Assembly.Location).ToLower();
            var versionInfo = FileVersionInfo.GetVersionInfo(location);
            var istage = interfaces[0];
            var types = istage.GenericTypeArguments;

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
    }
}
