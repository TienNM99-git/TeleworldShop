namespace TeleworldShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TeleworldShopDbContext dbContext;

        public TeleworldShopDbContext Init()
        {
            return dbContext ?? (dbContext = new TeleworldShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}