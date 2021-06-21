using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(TeleworldShop.Web.App_Start.Startup))]

namespace TeleworldShop.Web.App_Start
{
    public partial class Startup
    {
        public void ConfigHub(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}