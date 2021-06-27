using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TeleworldShop.Web.App_Start
{
    public class TeleworldHub : Hub
    {
        public void UpdateDashBoard()
        {
            Clients.All.UpdateDashBoard();
        }

        public void UpdateOrderList()
        {
            Clients.All.UpdateOrderList();
        }
    }
}