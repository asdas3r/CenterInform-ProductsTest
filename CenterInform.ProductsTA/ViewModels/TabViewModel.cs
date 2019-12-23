using System;
using System.Windows;

using Prism.Commands;
using Prism.Regions;

using CenterInform.ProductsTA.Interfaces;

namespace CenterInform.ProductsTA.ViewModels
{
    public abstract class TabViewModel : BaseViewModel, INavigationAware
    {
        public TabViewModel()
        {
            CloseCommand = new DelegateCommand(CloseCommandExecute);
        }

        private string _title;
        public string Title
        {
            //get { return _title; }
            get { return string.Format($"{_title} {_titleAppend}"); }
            set { SetProperty(ref _title, value); }
        }

        private string _titleAppend;
        public string TitleAppend
        {
            get { return _titleAppend; }
            set
            {
                SetProperty(ref _titleAppend, value);
                RaisePropertyChanged("Header");
            }
        }

        public DelegateCommand CloseCommand { get; }

        void CloseCommandExecute()
        {
            MessageBox.Show("Closing by Button");
            CurrentTabService.RemoveTab(this, false);
        }

        public bool CanClose { get; set; } = true;

        public ITabService CurrentTabService { get; set; }

        public object CurrentServiceObject { get; set; }

        public object InitialServiceObject { get; }

        public event EventHandler<TabCloseEventArgs> CloseAction;

        public void OnCloseInvokeAction()
        {
            CloseAction?.Invoke(this, new TabCloseEventArgs(CurrentServiceObject));
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }

    public class TabCloseEventArgs : EventArgs
    {
        public TabCloseEventArgs(object o)
        {
            TabObject = o;
        }

        public object TabObject { get; }
    }
}
