using System;
using System.Windows;

using Prism.Commands;

using CenterInform.ProductsTA.Models;
using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.ViewModels
{
    class RecurrentEntryDialogViewModel : BaseViewModel, IDialogCloseRequested
    {
        private bool repeatForEach = false;
        public RecurrentEntryDialogViewModel(Product toAdd, Product toSubstitute, int amountLeft)
        {
            ReccurentProduct = toAdd;
            ExistingProduct = toSubstitute;
            ReccurentCount = amountLeft;
            CancelEntry = new DelegateCommand(() => CloseRequested?.Invoke(this, new DialogCloseArgs(false) { CloseObject = RepeatForEach } ));
            PushEntry = new DelegateCommand(() => CloseRequested?.Invoke(this, new DialogCloseArgs(true) { CloseObject = RepeatForEach } ));
        }

        public event EventHandler<DialogCloseArgs> CloseRequested;

        public DelegateCommand CancelEntry { get; }

        public DelegateCommand PushEntry { get; }

        public bool RepeatForEach
        {
            get { return repeatForEach; }
            set { SetProperty(ref repeatForEach, value); }
        }

        public Visibility IsCheckboxVisible
        {
            get
            {
                if (ReccurentCount > 1)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public Product ReccurentProduct { get; }

        public Product ExistingProduct { get; }

        public string OldElementText
        {
            get
            {
                return $"Код продукта: {ExistingProduct.Code}, Наименование: {ExistingProduct.Name}, Описание: {ExistingProduct.Description}, Количество: {ExistingProduct.Quantity}";
            }
        }

        public string NewElementText
        {
            get
            {
                return $"Код продукта: {ReccurentProduct.Code}, Наименование: {ReccurentProduct.Name}, Описание: {ReccurentProduct.Description}, Количество: {ReccurentProduct.Quantity}";
            }
        }

        public int ReccurentCount { get; }
    }
}
