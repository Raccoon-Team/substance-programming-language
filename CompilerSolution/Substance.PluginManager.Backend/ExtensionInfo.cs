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

        public ExtensionInfo(string title, string description, params string[] authors)
        {
            Title = title;
            Description = description;
            Authors = authors;
        }

        public override string ToString()
        {
            return $"{nameof(Title)}:{Title}\n" +
                   $"{nameof(Description)}:{Description}";
        }
    }
}