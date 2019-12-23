using System;
using System.Drawing;
using System.Linq;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.ViewModels;

namespace CenterInform.ProductsTA.Services
{
    public class FileIOService : IFileIOService
    {
        private readonly IDialogWindowService _dialogService;

        public FileIOService(IDialogWindowService dialogService)
        {
            _dialogService = dialogService;
        }

        public T Import<T>(IFileDialogService dialogService, IFileService fileService)
        {
            try
            {
                if (dialogService.OpenFileDialog() == true)
                {
                    var items = fileService.GetFromFile<T>(dialogService.FilePath);

                    if (items == null || !string.IsNullOrEmpty(fileService.ErrorString))
                    {
                        _dialogService.ShowDialog("Ошибка", new CustomMessageBoxViewModel(fileService.ErrorString, SystemIcons.Error));
                        return default;
                    }

                    return items;
                }
            }
            catch (Exception e)
            {
                _dialogService.ShowDialog("Исключение", new CustomMessageBoxViewModel(e.Message, SystemIcons.Information));
            }

            return default;
        }

        public void Export<T>(IFileDialogService dialogService, IFileService fileService, T exportData)
        {
            if (exportData == default)
            {
                _dialogService.ShowDialog("Ошибка", new CustomMessageBoxViewModel("Нет данных для экспорта!", SystemIcons.Error));
                return;
            }

            try
            {
                if (dialogService.SaveFileDialog() == true)
                {
                    if (fileService.SaveToFile(dialogService.FilePath, exportData))
                    {
                        string result = string.Format("Экспорт прошел успешно! \nКонечный путь результирующего файла: \n {0}", dialogService.FilePath);
                        _dialogService.ShowDialog("Результат", new CustomMessageBoxViewModel(result, SystemIcons.Asterisk));
                    }
                    else
                    {
                        _dialogService.ShowDialog("Ошибка", new CustomMessageBoxViewModel(fileService.ErrorString, SystemIcons.Error));
                    }
                }
            }
            catch (Exception e)
            {
                _dialogService.ShowDialog("Исключение", new CustomMessageBoxViewModel(e.Message, SystemIcons.Information));
            }
        }
    }
}
