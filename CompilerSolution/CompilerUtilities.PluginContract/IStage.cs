using CompilerUtilities.Plugins.Versions;
using VersionInfo = CompilerUtilities.Plugins.Contract.Versions.VersionInfo;

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