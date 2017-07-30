using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = ClientConfiguration.LoadFromFile(Server.MapPath(@"~/ClientConfiguration.xml"));
            GrainClient.Initialize(config);
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
