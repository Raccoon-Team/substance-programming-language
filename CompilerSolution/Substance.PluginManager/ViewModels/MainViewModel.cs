using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using Substance.PluginManager.Models;

namespace Substance.PluginManager.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel()
        {
            ExtensionsCollection = new ObservableCollection<ExtensionModel>()
            {
                new ExtensionModel() {Title = "Example", Description = "Text text text"},
                new ExtensionModel() {Title = "Sample", Description = "Lorem ipsum, lorem"}
            };
        }

        private ObservableCollection<ExtensionModel> _extensionsCollection;

        public ObservableCollection<ExtensionModel> ExtensionsCollection
        {
            get => _extensionsCollection;
            set
            {
                _extensionsCollection = value;
                NotifyOfPropertyChange(() => ExtensionsCollection);
            }
        }

        private ExtensionModel _selectedExtension;

        public ExtensionModel SelectedExtension
        {
            get => _selectedExtension;
            set
            {
                _selectedExtension = value;
                NotifyOfPropertyChange(() => SelectedExtension);
            }
        }

        public void PluginsBtn_Click()
        {
            MessageBox.Show("Success!");
        }
    }
}