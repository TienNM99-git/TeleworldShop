using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }
    }
}