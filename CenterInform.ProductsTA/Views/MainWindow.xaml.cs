using System.Windows;

using Prism.Regions;
using Microsoft.Practices.ServiceLocation;

using CenterInform.ProductsTA.Core;

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

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _regionManager.Regions[RegionNames.TabRegion].RequestNavigate("ProductView");
        }
    }
}
