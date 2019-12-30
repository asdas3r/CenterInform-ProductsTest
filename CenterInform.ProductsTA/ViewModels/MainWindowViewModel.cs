using Prism.Commands;
using Prism.Regions;
using Microsoft.Practices.Unity;

using CenterInform.ProductsTA.Views;
using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Core;
using System;

namespace CenterInform.ProductsTA.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> Nabigate { get; set; }

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            Nabigate = new DelegateCommand<string>(Navigate);
        }

        void Navigate(string navigationPath)
        {
            _regionManager.Regions[RegionNames.TabRegion].RequestNavigate(navigationPath);
        }
    }
}
