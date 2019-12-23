using System.ComponentModel;

using Prism.Regions;
using Prism.Commands;

using CenterInform.ProductsTA.Models;
using CenterInform.ProductsTA.Services;
using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.ViewModels
{
    public class ProductAddEditViewModel : TabViewModel
    {
        private Product formProduct;
        private readonly ITabService _tabService;
        private readonly IDialogWindowService _dialogService;

        public ProductAddEditViewModel(ITabService tabService, IDialogWindowService dialogService) 
        {
            _tabService = tabService;
            CurrentTabService = _tabService;
            _dialogService = dialogService;
            CancelCommand = new DelegateCommand(() => CurrentTabService.RemoveTab(this, false));
            SaveCommand = new DelegateCommand(SaveCommandExecute, SaveCommandCanExecute);
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            Product recievedProduct = navigationContext.Parameters["serviceObject"] as Product;

            if (formProduct == null)
            {
                return true;
            }

            if (recievedProduct == null)
                return true;

            if (recievedProduct.Code.Equals(formProduct.Code))
                return false;

            return true;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Product recievedProduct = navigationContext.Parameters["serviceObject"] as Product;

            InitProductForm(recievedProduct);
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public string WindowLabel { get; set; }
        public bool IsIdReadOnly { get; set; }

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

            TitleAppend = FormProduct.Code;
        }

        private void SaveCommandExecute()
        {
            DbService dbService = new DbService();
            if (CurrentServiceObject == null)
            {
                CurrentServiceObject = FormProduct;
                if (dbService.FindValueSameId(FormProduct) != null)
                {
                    if (_dialogService.ShowDialog("Повторяющееся значение", new RecurrentEntryDialogViewModel(FormProduct, dbService.FindValueSameId(FormProduct), 1)) == true)
                    {
                        dbService.ModifyInDb(dbService.FindValueSameId(FormProduct), FormProduct);
                    }
                }
                else
                {
                    dbService.AddToDb(FormProduct);
                }
            }
            else
            {
                if (dbService.FindValueSameId(FormProduct) != null && !FormProduct.Code.Equals(((Product)CurrentServiceObject).Code))
                {
                    if (_dialogService.ShowDialog("Повторяющееся значение", new RecurrentEntryDialogViewModel(FormProduct, dbService.FindValueSameId(FormProduct), 1)) == true)
                    {
                        if (dbService.DeleteFromDb((Product)CurrentServiceObject))
                        {
                            dbService.ModifyInDb(dbService.FindValueSameId(FormProduct), FormProduct);
                        }
                    }
                }
                else
                {
                    dbService.ModifyInDb((Product)CurrentServiceObject, FormProduct);
                }
            }
            CurrentTabService.RemoveTab("ProductAddEditView", true);
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
    }
}
