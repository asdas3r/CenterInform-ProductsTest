using System.Collections.Generic;

using CenterInform.ProductsTA.Interfaces;
using CenterInform.ProductsTA.Models;

namespace CenterInform.ProductsTA.Services
{
    public class ProductUsageControlService : IObjectUsageControlService<Product>
    {
        private List<string> productsInUse = new List<string>();

        public void SetUsed(Product obj)
        {
            if (obj == null)
                return;

            var objKey = obj.Code;

            if (productsInUse.Contains(objKey))
                return;
            productsInUse.Add(objKey);
        }

        public void UnsetUsed(Product obj)
        {
            if (obj == null)
                return;

            var objKey = obj.Code;

            if (productsInUse.Contains(objKey))
                productsInUse.Remove(objKey);
        }

        public bool CheckIfUsed(Product obj)
        {
            if (obj == null)
                return false;

            var objKey = obj.Code;

            return productsInUse.Contains(objKey);
        }
    }
}
