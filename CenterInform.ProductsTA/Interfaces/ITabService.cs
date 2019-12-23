using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CenterInform.ProductsTA.ViewModels;

namespace CenterInform.ProductsTA.Interfaces
{
    public interface ITabService
    {
        void AddTab(string navigationPath);
        void AddTab(string navigationPath, object serviceObject);

        void RemoveTab(object tabObject, bool isDataChanged);
    }
}
