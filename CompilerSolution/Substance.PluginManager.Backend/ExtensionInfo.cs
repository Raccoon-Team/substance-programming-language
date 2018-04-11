using CompilerUtilities.Plugins.Contract.Versions;

namespace Substance.PluginManager.Backend
{
    public class ExtensionInfo
    {
        public string Title;
        public string Description;
        public string[] Authors;
        public string License;
        public string ProjectUrl;
        public string[] Tags;

        public VersionInfo Version;

        public ExtensionInfo(string title, string description, VersionInfo version, params string[] authors)
        {
            Title = title;
            Description = description;
            Authors = authors;
            Version = version;
        }
    }
}