namespace TeleworldShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePromotionAndDetailTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "ExpireDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Promotions", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promotions", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Promotions", "ExpireDate");
        }
    }
}
