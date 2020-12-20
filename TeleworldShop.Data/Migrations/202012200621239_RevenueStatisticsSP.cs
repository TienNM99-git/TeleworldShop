namespace TeleworldShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RevenueStatisticsSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetRevenueStatistics",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String()
                },
                    @"select 
                    o.CreatedDate as Date,
                    sum(od.Quantity*od.Price) as Revenues, 
                    sum(od.Quantity*od.Price - (od.Quantity*p.OriginalPrice)) as Benefit 
                    from Orders o
                    inner join OrderDetails od
                    on o.Id = od.OrderId
                    inner join Products p
                    on od.ProductId = p.Id
                    where o.CreatedDate <= cast(@toDate as date) and o.CreatedDate >= cast(@fromDate as date)
                    group by o.CreatedDate"
                );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevenueStatistics");
        }
    }
}
