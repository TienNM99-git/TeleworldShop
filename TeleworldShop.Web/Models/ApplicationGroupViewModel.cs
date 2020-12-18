using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeleworldShop.Web.Models;

namespace TeleworldShop.Web.Models
{
    public class ApplicationGroupViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ApplicationRoleViewModel> Roles { set; get; }
    }
}