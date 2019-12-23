using System;

using Prism.Commands;

using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.ViewModels
{
    class DeleteDialogViewModel : BaseViewModel, IDialogCloseRequested
    {
        public DeleteDialogViewModel()
        {
            CancelDelete = new DelegateCommand(() => CloseRequested?.Invoke(this, new DialogCloseArgs(false)));
            ConfirmDelete = new DelegateCommand(() => CloseRequested?.Invoke(this, new DialogCloseArgs(true)));
        }

        public event EventHandler<DialogCloseArgs> CloseRequested;
        public DelegateCommand CancelDelete { get; }
        public DelegateCommand ConfirmDelete { get; }
    }
}
