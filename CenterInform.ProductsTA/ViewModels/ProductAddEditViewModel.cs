using System;
using System.ComponentModel;

using Prism.Regions;
using Prism.Commands;
using Prism.Events;

using CenterInform.ProductsTA.Models;
using CenterInform.ProductsTA.Services;
using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Events;
using CenterInform.ProductsTA.Core;
using System.Linq;

namespace CenterInform.ProductsTA.ViewModels
{
    public class ProductAddEditViewModel : TabViewModel
    {
        private Product initProduct;
        private Product formProduct;
        private readonly IDialogWindowService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IObjectUsageControlService<Product> _usageControlService;

        public ProductAddEditViewModel(IRegionManager regionManager, IDialogWindowService dialogService, IEventAggregator eventAggregator, IObjectUsageControlService<Product> usageControlService) 
        {
            CurrentRegionManager = regionManager;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _usageControlService = usageControlService;

            CancelCommand = new DelegateCommand(CancelCommandExecute);
            SaveCommand = new DelegateCommand(SaveCommandExecute, SaveCommandCanExecute);

            _eventAggregator.GetEvent<ObjectChangedEvent>().Subscribe(OnProductChanged);
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public bool IsIdReadOnly { get; set; }

        private string _windowLabel;
        public string WindowLabel
        {
            get { return _windowLabel; }
            set { SetProperty(ref _windowLabel, value); }
        }

        public Product FormProduct
        {
            get { return formProduct; }

            set 
            {
                if (formProduct != null)
                {
                    FormProduct.PropertyChanged -= Product_PropertyChanged;
                }

                SetProperty(ref formProduct, value);

                if (formProduct != null)
                {
                    FormProduct.PropertyChanged += Product_PropertyChanged;
                }
            }
        }

        private void InitProductForm(Product product)
        {
            if (product == null)
            {
                Title = "Добавление";
                WindowLabel = "Добавление новой продукции";
                FormProduct = new Product();
                IsIdReadOnly = false;
            }
            else
            {
                Title = "Редактирование";
                WindowLabel = "Редактирование выбранной продукции";
                FormProduct = new Product(product.Code, product.Name, product.Description, product.Quantity);
                IsIdReadOnly = true;
            }

            initProduct = FormProduct;

            TitleAppend = FormProduct.Code;
        }

        private void SaveCommandExecute()
        {
            DbService dbService = new DbService();
            if (initProduct == null)
            {
                if (dbService.FindValueSameId(FormProduct) != null)
                {
                    if (_dialogService.ShowDialog("Повторяющееся значение", new RecurrentEntryDialogViewModel(FormProduct, dbService.FindValueSameId(FormProduct), 1)) == true)
                    {
                        dbService.ModifyInDb(dbService.FindValueSameId(FormProduct), FormProduct);
                        CloseUponSave(FormProduct);
                    }
                }
                else
                {
                    dbService.AddToDb(FormProduct);
                    CloseUponSave(FormProduct);
                }
            }
            else
            {
                if (dbService.FindValueSameId(FormProduct) != null && !FormProduct.Code.Equals(initProduct.Code))
                {
                    if (_dialogService.ShowDialog("Повторяющееся значение", new RecurrentEntryDialogViewModel(FormProduct, dbService.FindValueSameId(FormProduct), 1)) == true)
                    {
                        if (dbService.DeleteFromDb(initProduct))
                        {
                            dbService.ModifyInDb(dbService.FindValueSameId(FormProduct), FormProduct);
                            CloseUponSave(FormProduct);
                        }
                    }
                }
                else
                {
                    dbService.ModifyInDb(initProduct, FormProduct);
                    CloseUponSave(FormProduct);
                }
            }
        }

        private void CloseUponSave(Product product)
        {
            var tabItem = CurrentRegionManager.Regions[RegionNames.TabRegion].ActiveViews.FirstOrDefault();
            CloseCommand.Execute(tabItem);

            _eventAggregator.GetEvent<CloseTabEvent>().Publish(FormProduct);
        }

        private void CancelCommandExecute()
        {
            var tabItem = CurrentRegionManager.Regions[RegionNames.TabRegion].ActiveViews.FirstOrDefault();
            CloseCommand.Execute(tabItem);
        }

        protected override void OnCloseCommandExecute()
        {
            _usageControlService.UnsetUsed(initProduct);
            CurrentRegionManager.Regions[RegionNames.TabRegion].RequestNavigate("ProductView");
        }

        private bool SaveCommandCanExecute()
        {
            if (FormProduct == null)
                return false;
            return FormProduct.Error == null;
        }

        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Product.Code)))
            {
                TitleAppend = ((Product)sender).Code;
            }

            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnProductChanged(object obj)
        {
            if (obj == null)
                return;

            Product product = obj as Product;
            if (product == null)
                return;

            if (product.Code.Equals(initProduct.Code))
            {
                FormProduct = new Product(product.Code, product.Name, product.Description, product.Quantity);
                initProduct = FormProduct;
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Product recievedProduct = navigationContext.Parameters["selectedProduct"] as Product;

            if (formProduct == null)
            {
                return false;
            }

            if (recievedProduct == null)
                return false;

            if (recievedProduct.Code.Equals(formProduct.Code))
                return true;

            return false;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!IsBeenNavigated)
            {
                Product recievedProduct = navigationContext.Parameters["selectedProduct"] as Product;

                InitProductForm(recievedProduct);

                IsBeenNavigated = true;
            }
        }
    }
}
