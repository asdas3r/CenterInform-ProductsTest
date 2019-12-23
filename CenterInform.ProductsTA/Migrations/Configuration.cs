using System.Data.Entity.Migrations;

using CenterInform.ProductsTA.Models;

namespace CenterInform.ProductsTA.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ProductDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProductDBContext context) { }
    }
}
