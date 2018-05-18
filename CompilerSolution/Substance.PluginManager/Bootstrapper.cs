using System.Windows;
using Caliburn.Micro;
using Substance.PluginManager.ViewModels;

namespace Substance.PluginManager
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}