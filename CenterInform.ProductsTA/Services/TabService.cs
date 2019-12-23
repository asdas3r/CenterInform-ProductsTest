using Prism.Mvvm;
using Prism.Regions;
using System.Windows;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Core;
using System.Linq;

namespace CenterInform.ProductsTA.Services
{
    public class TabService : BindableBase, ITabService
    {
        private readonly IRegionManager _regionManager;
        private readonly string _regionName = RegionNames.TabRegion;

        public TabService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void AddTab(string navigationPath)
        {
            _regionManager.RequestNavigate(_regionName, navigationPath);
        }

        public void AddTab(string navigationPath, object serviceObject)
        {
            var p = new NavigationParameters();
            p.Add("serviceObject", serviceObject);
            _regionManager.RequestNavigate(_regionName, navigationPath);
        }

        public void RemoveTab(object tabObject, bool isDataChanged)
        {
            //tbd
            MessageBox.Show("Removing");
        }

    }
}
