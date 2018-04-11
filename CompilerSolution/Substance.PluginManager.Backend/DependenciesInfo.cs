using CompilerUtilities.Plugins.Contract.Versions;

namespace Substance.PluginManager.Backend
{
    public class DependenciesInfo
    {
        public string Title;
        public VersionInfo RequireVersion;
        public DependenciesCondition Condition;

        public DependenciesInfo(string title, VersionInfo requireVersion, DependenciesCondition condition)
        {
            Title = title;
            RequireVersion = requireVersion;
            Condition = condition;
        }
    }
}