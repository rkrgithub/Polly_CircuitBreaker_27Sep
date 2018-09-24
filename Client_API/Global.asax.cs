using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Client_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "My Log File.txt"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "My Log File.txt");
        }
    }
}
