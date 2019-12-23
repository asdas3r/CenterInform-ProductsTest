using System.Data.Entity;

namespace CenterInform.ProductsTA.Models
{
    class ProductDBContext : DbContext
    {
        public ProductDBContext() : base("DBConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductDBContext, Migrations.Configuration>());
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductDBContext, Migrations.Configuration>());
        }
    }
}
