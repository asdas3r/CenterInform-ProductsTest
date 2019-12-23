using System.Windows;

namespace CenterInform.ProductsTA.Views
{
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            Owner = Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
