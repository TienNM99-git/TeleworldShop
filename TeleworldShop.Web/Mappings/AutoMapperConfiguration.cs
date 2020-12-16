using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeleworldShop.Model.Models;
using TeleworldShop.Web.Models;

namespace TeleworldShop.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IConfigurationProvider Configure()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductTag, ProductTagViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            });
            return config;
            //IMapper mapper = config.CreateMapper();
        }
    }
}