using System.Windows;

using Microsoft.Practices.ServiceLocation;
using Prism.Regions;

namespace CenterInform.ProductsTA.Views
{
    public partial class MainWindow : Window
    {
        private readonly IRegionManager _regionManager;

        public MainWindow()
        {
            InitializeComponent();

            _regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            RegionManager.SetRegionManager(TabControl, _regionManager);
        }
    }
}
