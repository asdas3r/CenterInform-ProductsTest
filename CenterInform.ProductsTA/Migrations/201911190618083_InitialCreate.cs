namespace CenterInform.ProductsTA.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductInfo",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 8),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 2000),
                        Quantity = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductInfo");
        }
    }
}
