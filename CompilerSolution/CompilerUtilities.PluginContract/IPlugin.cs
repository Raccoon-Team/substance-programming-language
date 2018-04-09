using CompilerUtilities.Plugins.Versions;

namespace CompilerUtilities.Plugins.Contract
{
    public interface IPlugin<T>
    {
        VersionInfo VersionInfo { get; }
        VersionInfo RequreCompilerVersion { get; }

        string Name { get; }
        string Author { get; }
        string Description { get; }

        T Activate(T input);
    }
}