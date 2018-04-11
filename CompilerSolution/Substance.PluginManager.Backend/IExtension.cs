using System;
using Substance.PluginManager.Backend.Configs;

namespace Substance.PluginManager.Backend
{
    public interface IExtension
    {
        Configuration Configuration { get; }
        ExtensionInfo Info { get; }
        string ExtensionFolder { get; }
        ExtensionStatus Status { get; }
        Type[] Types { get; }
    }
}