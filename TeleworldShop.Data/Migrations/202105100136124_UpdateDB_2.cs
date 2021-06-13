namespace TeleworldShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromotionDetails", "CategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.PromotionDetails", "PromotionId", "dbo.Promotions");
            DropForeignKey("dbo.PromotionDetails", "TagId", "dbo.Tags");
            DropIndex("dbo.PromotionDetails", new[] { "PromotionId" });
            DropIndex("dbo.PromotionDetails", new[] { "CategoryId" });
            DropIndex("dbo.PromotionDetails", new[] { "TagId" });
            DropPrimaryKey("dbo.PromotionDetails");
            AddPrimaryKey("dbo.PromotionDetails", new[] { "PromotionId", "CategoryId" });
            DropColumn("dbo.PromotionDetails", "TagId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromotionDetails", "TagId", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropPrimaryKey("dbo.PromotionDetails");
            AddPrimaryKey("dbo.PromotionDetails", new[] { "PromotionId", "CategoryId", "TagId" });
            CreateIndex("dbo.PromotionDetails", "TagId");
            CreateIndex("dbo.PromotionDetails", "CategoryId");
            CreateIndex("dbo.PromotionDetails", "PromotionId");
            AddForeignKey("dbo.PromotionDetails", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionDetails", "PromotionId", "dbo.Promotions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PromotionDetails", "CategoryId", "dbo.ProductCategories", "Id", cascadeDelete: true);
        }
    }
}
