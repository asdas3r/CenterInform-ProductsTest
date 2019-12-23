using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterInform.ProductsTA.Interfaces
{
    public interface IFileIOService
    {
        T Import<T>(IFileDialogService dialogService, IFileService fileService);

        void Export<T>(IFileDialogService dialogService, IFileService fileService, T exportData);
    }
}
