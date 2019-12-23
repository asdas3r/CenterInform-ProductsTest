using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Views;

namespace CenterInform.ProductsTA.Services
{
    public class DialogWindowService : IDialogWindowService
    {
        public bool? ShowDialog<TViewModel>(string title, TViewModel viewModel) where TViewModel : IDialogCloseRequested
        {
            DialogWindow dialog = new DialogWindow();

            viewModel.CloseRequested += (sender, e) =>
            {
                if (e.CloseResult.HasValue)
                {
                    dialog.DialogResult = e.CloseResult;
                }
                else
                {
                    dialog.Close();
                }
            };

            dialog.Title = title;
            dialog.DataContext = viewModel;

            return dialog.ShowDialog();
        }
    }
}
