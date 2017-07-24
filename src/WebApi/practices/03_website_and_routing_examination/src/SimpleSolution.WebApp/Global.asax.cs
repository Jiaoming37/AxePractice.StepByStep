using System;
using System.Web;
using System.Web.Http;

namespace SimpleSolution.WebApp
{
    public class Global : HttpApplication
    {
        /*when one app can run on web, need this app start config as entrance, so must need add new Item to Global.asax*/
        protected void Application_Start(object sender, EventArgs e)
        {
            var config = GlobalConfiguration.Configuration;
            Bootstrapper.Init(config);
        }
    }
}