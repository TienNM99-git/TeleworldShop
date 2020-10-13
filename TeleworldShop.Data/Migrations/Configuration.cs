namespace TeleworldShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TeleworldShop.Model.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TeleworldShop.Data.TeleworldShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TeleworldShop.Data.TeleworldShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new TeleworldShopDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new TeleworldShopDbContext()));
            var user = new ApplicationUser()
            {
                UserName = "tiennm",
                Email = "tiennm1999@gmail.com",
                EmailConfirmed = true,
                BirthDay = new DateTime(1999, 3, 28),
                FullName = "Nguyen Manh Tien",
                PhoneNumber = "0362537131",
                PhoneNumberConfirmed = true,
                Address = "51/39 10th Street, Linh Chieu Ward, Thu Duc District, Ho Chi Minh city",
            };
            manager.Create(user, "TienNM1999");
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }
            var adminUser = manager.FindByEmail("tiennm1999@gmail.com");
            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }
    }
}
