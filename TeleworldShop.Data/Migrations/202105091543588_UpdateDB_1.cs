namespace TeleworldShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDB_1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromotionDetails", "TagId", "dbo.Tags");
            DropIndex("dbo.PromotionDetails", new[] { "TagId" });
            DropPrimaryKey("dbo.PromotionDetails");
            AddColumn("dbo.PromotionDetails", "TagName", c => c.String());
            AddPrimaryKey("dbo.PromotionDetails", new[] { "PromotionId", "CategoryId" });
            DropColumn("dbo.PromotionDetails", "TagId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromotionDetails", "TagId", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropPrimaryKey("dbo.PromotionDetails");
            DropColumn("dbo.PromotionDetails", "TagName");
            AddPrimaryKey("dbo.PromotionDetails", new[] { "PromotionId", "CategoryId", "TagId" });
            CreateIndex("dbo.PromotionDetails", "TagId");
            AddForeignKey("dbo.PromotionDetails", "TagId", "dbo.Tags", "Id", cascadeDelete: true);
        }
    }
}
