namespace Substance.PluginManager.Backend
{
    public class ExtensionInfo
    {
        public string Title;
        public string Description;
        public string Author;
        public string License;
        public string ProjectUrl;
        public string[] Tags;

        public ExtensionInfo(string title, string description, string author)
        {
            Title = title;
            Description = description;
            Author = author;
        }

        public override string ToString()
        {
            return $"{nameof(Title)}:{Title}\n" +
                   $"{nameof(Description)}:{Description}";
        }
    }
}