using System;
using System.IO;
using Microsoft.Win32;
using System.Drawing;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.ViewModels;

namespace CenterInform.ProductsTA.Services
{
    class FileDialogService : IFileDialogService
    {
        private readonly IDialogWindowService _dialogService;

        public FileDialogService(IDialogWindowService dialogService)
        {
            _dialogService = dialogService;
        }

        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "xml";
            openFile.Filter = "XML Files (*.xml) | *.xml";

            if (openFile.ShowDialog() == true)
            {
                if (!string.Equals(Path.GetExtension(openFile.FileName), ".xml", StringComparison.OrdinalIgnoreCase))
                {
                    _dialogService.ShowDialog("Ошибка", new CustomMessageBoxViewModel("Ошибка формата исходного файла.", SystemIcons.Error));
                    return false;
                }
                else
                {
                    FilePath = openFile.FileName;
                    return true;
                }
            }

            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "xml";
            saveFile.Filter = "XML Files (*.xml) | *.xml";

            if (saveFile.ShowDialog() == true)
            {
                if (!string.Equals(Path.GetExtension(saveFile.FileName), ".xml", StringComparison.OrdinalIgnoreCase))
                {
                    FilePath = string.Format(saveFile.FileName + ".xml");
                }
                else
                {
                    FilePath = saveFile.FileName;
                }
                return true;
            }

            return false;
        }
    }
}
