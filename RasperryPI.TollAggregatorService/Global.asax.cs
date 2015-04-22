using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace RasperryPI.TollAggregatorService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // New code
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
