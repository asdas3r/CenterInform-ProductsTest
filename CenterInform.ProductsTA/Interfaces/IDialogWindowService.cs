using System;

namespace CenterInform.ProductsTA.Interfaces
{
    public interface IDialogWindowService
    {
        bool? ShowDialog<TViewModel>(string title, TViewModel viewModel) where TViewModel : IDialogCloseRequested;
    }

    public interface IDialogCloseRequested
    {
        event EventHandler<DialogCloseArgs> CloseRequested;
    }

    public class DialogCloseArgs : EventArgs
    {
        public DialogCloseArgs(bool? result)
        {
            CloseResult = result;
        }

        public bool? CloseResult { get; }

        public bool? CloseObject { get; set; }
    }
}
