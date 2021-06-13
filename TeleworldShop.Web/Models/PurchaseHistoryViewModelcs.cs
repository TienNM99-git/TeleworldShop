using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeleworldShop.Web.Models
{
    public class PurchaseHistoryViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public decimal Total { set; get; }

        public string PaymentStatus { set; get; }

        public DateTime CreatedDate { set; get; }

        public bool Status { set; get; }

    }
}