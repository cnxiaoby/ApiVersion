using ApiVersion.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ApiVersion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 使用自定义的 ControllerFactory
            ControllerBuilder.Current.SetControllerFactory(typeof(VersionControllerFactory));
        }
    }
}
