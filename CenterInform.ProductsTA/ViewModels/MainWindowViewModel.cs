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
        private readonly ITabService _tabService;
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> Nabigate { get; set; }

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer container, ITabService tabService)
        {
            _regionManager = regionManager;
            _tabService = tabService;
            _regionManager.RegisterViewWithRegion(RegionNames.TabRegion, typeof(ProductView));
            Nabigate = new DelegateCommand<string>(Navigate);
        }

        void Navigate(string navigationPath)
        {
            _tabService.AddTab(navigationPath);
        }
    }
}
