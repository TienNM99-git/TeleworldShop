using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleworldShop.Common.ViewModels
{
    public class PurchaseHistoryViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Total { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

        public string OrderStatus { get; set; }
    }
}
