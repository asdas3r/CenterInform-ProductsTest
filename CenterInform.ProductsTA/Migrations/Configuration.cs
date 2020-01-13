using System.Data.Entity.Migrations;

using CenterInform.ProductsTA.Models;

namespace CenterInform.ProductsTA.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ProductDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProductDbContext context) { }
    }
}
