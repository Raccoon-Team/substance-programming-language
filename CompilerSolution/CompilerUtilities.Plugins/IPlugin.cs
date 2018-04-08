using CompilerUtilities.Plugins.Versions;

namespace CompilerUtilities.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }
        string Description { get; }

        uint Priority { get; }

        VersionInfo Version { get; }
        VersionInfo RequreCompilerVersion { get; }

        void Activate(IPluginManager manager);
    }
}