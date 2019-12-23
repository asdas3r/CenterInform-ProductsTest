using System.Windows;

using Prism.Unity;
using Prism.Modularity;
using Prism.Mvvm;
using Microsoft.Practices.Unity;

using CenterInform.ProductsTA.Views;
using CenterInform.ProductsTA.ViewModels;
using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Services;

namespace CenterInform.ProductsTA
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(MainModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => Container.Resolve(type));
            
            Container.RegisterType<ITabService, TabService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDialogWindowService, DialogWindowService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFileIOService, FileIOService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFileDialogService, FileDialogService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFileService, XmlService>(new ContainerControlledLifetimeManager());
        }
    }
}
