using System.Drawing;

namespace CenterInform.ProductsTA.Interfaces
{
    public interface IFileDialogService
    {
        string FilePath { get; set; }

        bool OpenFileDialog();

        bool SaveFileDialog();
    }
}
