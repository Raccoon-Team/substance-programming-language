namespace Substance.PluginManager.Models
{
    public class ExtensionModel : IListItem
    {
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}