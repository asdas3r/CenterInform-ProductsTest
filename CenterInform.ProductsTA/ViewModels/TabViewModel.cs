using Prism.Commands;
using Prism.Regions;

using CenterInform.ProductsTA.Core;

namespace CenterInform.ProductsTA.ViewModels
{
    public abstract class TabViewModel : BaseViewModel, INavigationAware
    {
        public TabViewModel()
        {
            CloseCommand = new DelegateCommand<object>(CloseCommandExecute);
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

        public DelegateCommand<object> CloseCommand { get; }

        private void CloseCommandExecute(object tabItem)
        {
            if (!CanClose)
                return;
            CurrentRegionManager.Regions[RegionNames.TabRegion].Remove(tabItem);
            OnCloseCommandExecute();
        }

        protected virtual void OnCloseCommandExecute(object serviceObject = null)
        {

        }

        public IRegionManager CurrentRegionManager { get; protected set; }

        public bool CanClose { get; set; } = true;

        public bool IsBeenNavigated { get; set; } = false;

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
