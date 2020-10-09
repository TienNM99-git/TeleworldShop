using System;

namespace TeleworldShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TeleworldShopDbContext Init();
    }
}