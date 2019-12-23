using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Prism.Commands;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Models;
using CenterInform.ProductsTA.Services;
using Prism.Regions;

namespace CenterInform.ProductsTA.ViewModels
{
    public class ProductViewModel : TabViewModel
    {
        private readonly ITabService _tabService;
        private readonly IDialogWindowService _dialogService;
        private readonly IFileIOService _fileIOService;
        private readonly IFileDialogService _fileDialog;
        private readonly IFileService _xmlService;
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ReadOnlyObservableCollection<Product> ProductCollection { get; }

        public ProductViewModel(ITabService tabService, IDialogWindowService dialogService, IFileIOService fileIOService, IFileDialogService fileDialog, IFileService fileService)
        {
            Title = "Список товаров";
            ProductCollection = new ReadOnlyObservableCollection<Product>(_products);
            CanClose = false;

            _tabService = tabService;
            CurrentTabService = _tabService;
            _dialogService = dialogService;
            _fileIOService = fileIOService;
            _fileDialog = fileDialog;
            _xmlService = fileService;

            SelectPreviousProductCommand = new DelegateCommand(SelectPreviousProductCommandExecute);
            SelectNextProductCommand = new DelegateCommand(SelectNextProductCommandExecute);
            CreateProductCommand = new DelegateCommand(CreateProductCommandExecute);
            EditProductCommand = new DelegateCommand(EditProductCommandExecute, EditProductCommandCanExecute).ObservesProperty(() => ProductCollection.Count);
            RemoveProductCommand = new DelegateCommand(RemoveProductCommandExecute, RemoveProductCommandCanExecute).ObservesProperty(() => ProductCollection.Count);
            ImportXmlCommand = new DelegateCommand(ImportXmlCommandExecute);
            ExportXmlCommand = new DelegateCommand(ExportXmlCommandExecute);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            RefreshSource();
            if (ProductCollection.Count > 0)
            {
                SelectedProduct = ProductCollection[0];
            }
        }

        public DelegateCommand SelectPreviousProductCommand { get; }
        public DelegateCommand SelectNextProductCommand { get; }
        public DelegateCommand CreateProductCommand { get; }
        public DelegateCommand EditProductCommand { get; }
        public DelegateCommand RemoveProductCommand { get; }
        public DelegateCommand ImportXmlCommand { get; }
        public DelegateCommand ExportXmlCommand { get; } 

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { SetProperty(ref selectedProduct, value); }
        }

        private void SelectPreviousProductCommandExecute()
        {
            int index = ProductCollection.IndexOf(SelectedProduct) - 1;

            if (index >= 0)
            {
                SelectedProduct = ProductCollection[index];
            }
        }

        private void SelectNextProductCommandExecute()
        {
            int index = ProductCollection.IndexOf(SelectedProduct) + 1;

            if (index < ProductCollection.Count)
            {
                SelectedProduct = ProductCollection[index];
            }
        }

        private void CreateProductCommandExecute()
        {
            CurrentTabService.AddTab("ProductAddEditView", null);
        }

        private void EditProductCommandExecute()
        {
            CurrentTabService.AddTab("ProductAddEditView", SelectedProduct);
            RemoveProductCommand.RaiseCanExecuteChanged();
        }

        private bool EditProductCommandCanExecute()
        {
            return ProductCollection.Count > 0;
        }

        private void RemoveProductCommandExecute()
        {
            Product product = SelectedProduct;
            if (product != null)
            {
                if (_dialogService.ShowDialog("Удаление записи", new DeleteDialogViewModel()) == true)
                {
                    if (new DbService().DeleteFromDb(product))
                    {
                        int index = ProductCollection.IndexOf(SelectedProduct);

                        RefreshSource();

                        if (ProductCollection.Count > 0)
                        {
                            if (index > 0)
                            {
                                index--;
                            }
                            SelectedProduct = ProductCollection[index];
                        }
                    }
                }
            }
        }

        private bool RemoveProductCommandCanExecute()
        {
            if (ProductCollection.Count > 0 )
            {
                return true;
            }
            return false;
        }

        private void ImportXmlCommandExecute()
        {
            FileDialogService dialog = _fileDialog as FileDialogService;
            XmlService xml = _xmlService as XmlService;

            var products = _fileIOService.Import<Products>(dialog, xml);
            if (products != null)
            {
                ImportListWithChoices(products.productsList);
            }
        }

        private void ExportXmlCommandExecute()
        {
            FileDialogService dialog = _fileDialog as FileDialogService;
            XmlService xml = _xmlService as XmlService;
            Products products = null;

            if (ProductCollection.Count != 0)
            {
                products = new Products(ProductCollection.ToList());
            }

            _fileIOService.Export(dialog, xml, products);
        }

        private void ImportListWithChoices(List<Product> productsList)
        {
            DbService dbService = new DbService();
            int amount = 0;

            foreach (var p in productsList)
            {
                if (dbService.FindValueSameId(p) != null)
                    amount++;
            }

            bool repeatForEach = false;
            bool? dialogChoice = null;

            foreach (var p in productsList)
            {
                if (dbService.FindValueSameId(p) != null)
                {
                    var entryVm = new RecurrentEntryDialogViewModel(p, dbService.FindValueSameId(p), amount);

                    entryVm.CloseRequested += (sender, e) =>
                    {
                        if ((e.CloseObject).HasValue)
                        {
                            repeatForEach = (bool)e.CloseObject;
                        }
                    };

                    if (!repeatForEach)
                    {
                        dialogChoice = _dialogService.ShowDialog("Повторяющееся значение", entryVm);
                    }

                    if (dialogChoice == true)
                    {
                        dbService.ModifyInDb(dbService.FindValueSameId(p), p);
                        RefreshSource();
                        SelectedProduct = ProductCollection.First(x => x.Code.Equals(p.Code));
                    }

                    amount--;
                }
                else
                {
                    dbService.AddToDb(p);
                    RefreshSource();
                    SelectedProduct = ProductCollection.First(x => x.Code.Equals(p.Code));
                }
            }
        }

        private void OnTabClose(object sender, TabCloseEventArgs e)
        {
            var returnedObject = e.TabObject as Product;

            if (returnedObject == null)
            {
                return;
            }

            RefreshSource();

            if (ProductCollection.Count == 0)
            {
                return;
            }

            SelectedProduct = ProductCollection.First();

            foreach (var p in ProductCollection)
            {
                if (p.Code.Equals(returnedObject.Code))
                {
                    SelectedProduct = p;
                }
            }
        }

        private void RefreshSource()
        {
            _products.Clear();
            var returnedProducts = new DbService().ReadFromDb;
            foreach (var p in returnedProducts)
            {
                _products.Add(p);
            }
        }
    }
}
