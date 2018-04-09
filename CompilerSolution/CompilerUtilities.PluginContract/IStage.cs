using CompilerUtilities.PluginContract;
using CompilerUtilities.Plugins.Versions;

namespace CompilerUtilities.Plugins.Contract
{
    public interface IStage<in TIn, out TOut>
    {
        VersionInfo VersionInfo { get; }
        VersionInfo RequreCompilerVersion { get; }

        string Name { get; }
        string Author { get; }
        string Description { get; }

        void Initialize(ICompileOptions options);

        TOut Process(TIn input);
    }
}