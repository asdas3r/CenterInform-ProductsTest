using Prism.Modularity;
using Microsoft.Practices.Unity;

using CenterInform.ProductsTA.Views;
using CenterInform.ProductsTA.Services;
using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Models;

namespace CenterInform.ProductsTA
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer _container;

        public MainModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            //_container.RegisterType<object, ProductView>("ProductView");
            _container.RegisterType<object, ProductAddEditView>("ProductAddEditView");
            _container.RegisterType<object, CustomMessageBoxView>("CustomMessageBoxView");
            _container.RegisterType<object, DeleteDialogView>("DeleteDialogView");
            _container.RegisterType<object, DialogWindow>("DialogWindow");
            _container.RegisterType<object, RecurrentEntryDialogView>("RecurrentEntryDialogView");

        }
    }
}
