using System;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Prism.Commands;

using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.ViewModels
{
    class CustomMessageBoxViewModel : BaseViewModel, IDialogCloseRequested
    {
        private readonly Icon recievedIcon;

        public CustomMessageBoxViewModel(string message, Icon icon)
        {
            Message = message;
            recievedIcon = icon;
            ImageVisibility = (recievedIcon == null) ? Visibility.Collapsed : Visibility.Visible;
            OkCommand = new DelegateCommand(() => CloseRequested?.Invoke(this, new DialogCloseArgs(true)));
        }

        public event EventHandler<DialogCloseArgs> CloseRequested;

        public DelegateCommand OkCommand { get; }

        public Visibility ImageVisibility { get; } 

        public string Message { get; }

        public ImageSource MessageImageSource
        {
            get
            {
                if (recievedIcon != null)
                    return Imaging.CreateBitmapSourceFromHIcon(recievedIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                else
                    return null;
            }
        }
    }
}
