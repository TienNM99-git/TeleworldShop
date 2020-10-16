namespace TeleworldShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using TeleworldShop.Common;
    using TeleworldShop.Model.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TeleworldShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TeleworldShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            CreateProductCategorySample(context);
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

        private void CreateProductCategorySample(TeleworldShopDbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory() { Name="Phone",Alias="phone",Status=true },
                    new ProductCategory() { Name="Laptop",Alias="laptop",Status=true },
                    new ProductCategory() { Name="Tablet",Alias="tablet",Status=true },
                    new ProductCategory() { Name="Accessories",Alias="accessories",Status=true },
                    new ProductCategory() { Name="Watch",Alias="watch",Status=true },
                    new ProductCategory() { Name="Smart Watch",Alias="smart-watch",Status=true },
                    new ProductCategory() { Name="PC/Printing Machine",Alias="pc-printing",Status=true },
                    new ProductCategory() { Name="SIM",Alias="sim",Status=true }
                };
                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }

        private void CreateFooter(TeleworldShopDbContext context)
        {
            if (context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
                string content = "";
            }
        }

        private void CreateSlide(TeleworldShopDbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide() {
                        Name ="Slide 1",
                        DisplayOrder =1,
                        Status =true,
                        Url ="#",
                        Image ="/Assets/client/images/bag.jpg",
                        Content =@"	<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur
                            adipisicing elit, sed do eiusmod tempor incididunt ut labore et </ p >
                        <span class=""on-get"">GET NOW</span>" },
                    new Slide() {
                        Name ="Slide 2",
                        DisplayOrder =2,
                        Status =true,
                        Url ="#",
                        Image ="/Assets/client/images/bag1.jpg",
                    Content=@"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>

                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </ p >

                                <span class=""on-get"">GET NOW</span>"},
                };
                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }

        private void CreatePage(TeleworldShopDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                try
                {
                    var page = new Page()
                    {
                        Name = "Giới thiệu",
                        Alias = "gioi-thieu",
                        Content = @"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium ",
                        Status = true
                    };
                    context.Pages.Add(page);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }
            }
        }

        //private void CreateContactDetail(TeleworldShopDbContext context)
        //{
        //    if (context.ContactDetails.Count() == 0)
        //    {
        //        try
        //        {
        //            var contactDetail = new TeleworldShop.Model.Models.ContactDetail()
        //            {
        //                Name = "Shop thời trang TEDU",
        //                Address = "Ngõ 77 Xuân La",
        //                Email = "tedu@gmail.com",
        //                Lat = 21.0633645,
        //                Lng = 105.8053274,
        //                Phone = "095423233",
        //                Website = "http://tedu.com.vn",
        //                Other = "",
        //                Status = true
        //            };
        //            context.ContactDetails.Add(contactDetail);
        //            context.SaveChanges();
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            foreach (var eve in ex.EntityValidationErrors)
        //            {
        //                Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
        //                }
        //            }
        //        }
        //    }
        //}
    }
}