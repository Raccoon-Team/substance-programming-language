using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using Substance.PluginManager.Models;

namespace Substance.PluginManager.ViewModels
{
    public class MainViewModel : Screen
    {
        private ObservableCollection<IListItem> _extensionsCollection;

        private ExtensionModel _selectedExtension;

        public MainViewModel()
        {
            ExtensionsCollection = new ObservableCollection<IListItem>
            {
                new ExtensionModel {Title = "Example", Description = "Text text text"},
                new ExtensionModel {Title = "Sample", Description = "Lorem ipsum, lorem"}
            };
        }

        public ObservableCollection<IListItem> ExtensionsCollection
        {
            get => _extensionsCollection;
            set
            {
                _extensionsCollection = value;
                NotifyOfPropertyChange(() => ExtensionsCollection);
            }
        }

        public ExtensionModel SelectedExtension
        {
            get => _selectedExtension;
            set
            {
                _selectedExtension = value;
                NotifyOfPropertyChange(() => SelectedExtension);
            }
        }

        #region Event Handlers

        public void PluginsBtn_Click()
        {
            MessageBox.Show("Success!");
        }

        public void SearchBtn_Click()
        {
            MessageBox.Show("Searching...");
        }

        public void BuildPanelBtn_Click()
        {
            MessageBox.Show("Build panel opening");
        }

        public void SettingsPanelBtn_Click()
        {
            MessageBox.Show("Settings panel opening");
        }

        public void DownloadsPanelBtn_Click()
        {
            MessageBox.Show("Downloads panel opening");
        }

        #endregion
    }
}