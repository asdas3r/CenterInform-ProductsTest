using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using CenterInform.ProductsTA.Models;

namespace CenterInform.ProductsTA.Services
{
    public class DbService
    {
        public void AddToDb(Product product)
        {
            using (var db = new ProductDbContext())
            {
                if (product != null)
                {
                    if (!db.Products.Any(p => p.Code.Equals(product.Code)))
                    {
                        db.Products.Add(product);
                        db.SaveChanges();
                    }
                }
            }
        }

        public List<Product> ReadFromDb
        {
            get
            {
                using (var db = new ProductDbContext())
                {
                    return db.Products.ToList();
                }
            }
        }

        public bool DeleteFromDb(Product product)
        {
            using (var db = new ProductDbContext())
            {
                if (product != null)
                {
                    db.Products.Attach(product);
                    db.Products.Remove(product);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public void ModifyInDb(Product sourceProduct, Product destProduct)
        {
            using (var db = new ProductDbContext())
            {
                if (sourceProduct != null && destProduct != null && (!db.Products.Any(p => p.Code.Equals(destProduct.Code)) | destProduct.Code.Equals(sourceProduct.Code) == true))
                {
                    db.Products.Attach(sourceProduct);

                    sourceProduct.Code = destProduct.Code;
                    sourceProduct.Name = destProduct.Name;
                    sourceProduct.Description = destProduct.Description;
                    sourceProduct.Quantity = destProduct.Quantity;

                    db.SaveChanges();
                }
            }
        }

        public Product FindValueSameId(Product product)
        {
            using (var db = new ProductDbContext())
            {
                Product p = null;
                p = db.Products.Find(product.Code);
                return p;
            }
        }

        public ObservableCollection<Product> GetProducts()
        {
            using (var db = new ProductDbContext())
            {
                var t = new ObservableCollection<Product>(db.Products.ToList());
                return t;
            }
        }
    }
}
