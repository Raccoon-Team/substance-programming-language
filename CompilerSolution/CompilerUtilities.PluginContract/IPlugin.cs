using CompilerUtilities.Plugins.Contract.Versions;

namespace CompilerUtilities.Plugins.Contract
{
    public interface IPlugin<T>
    {
        VersionInfo Version { get; }
        VersionInfo RequreCompilerVersion { get; }

        string Name { get; }
        string Author { get; }
        string Description { get; }

        T Activate(T input);
    }
}