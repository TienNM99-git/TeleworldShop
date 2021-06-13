namespace TeleworldShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {            
            CreateTable(
                "dbo.PromotionDetails",
                c => new
                    {
                        PromotionId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        TagId = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.PromotionId, t.CategoryId, t.TagId })
                .ForeignKey("dbo.ProductCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Promotions", t => t.PromotionId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PromotionId)
                .Index(t => t.CategoryId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        PromotionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Apply = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionDetails", "TagId", "dbo.Tags");
            DropForeignKey("dbo.PromotionDetails", "PromotionId", "dbo.Promotions");
            DropForeignKey("dbo.PromotionDetails", "CategoryId", "dbo.ProductCategories");
            DropIndex("dbo.PromotionDetails", new[] { "TagId" });
            DropIndex("dbo.PromotionDetails", new[] { "CategoryId" });
            DropIndex("dbo.PromotionDetails", new[] { "PromotionId" });
            DropTable("dbo.Promotions");
            DropTable("dbo.PromotionDetails");
        }
    }
}
