using CompilerUtilities.Plugins.Contract.Versions;

namespace CompilerUtilities.Plugins.Contract
{
    public interface ICompilerExtension
    {
        VersionInfo Version { get; }
        VersionInfo RequreCompilerVersion { get; }

        string Name { get; }
        string Author { get; }
        string Description { get; }
    }
}