using System.Data.Entity;

namespace CenterInform.ProductsTA.Models
{
    class ProductDbContext : DbContext
    {
        public ProductDbContext() : base("DBConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductDbContext, Migrations.Configuration>());
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductDbContext, Migrations.Configuration>());
        }
    }
}
