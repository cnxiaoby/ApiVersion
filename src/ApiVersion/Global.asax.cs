using ApiVersion.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ApiVersion
{
    /// <summary>
    /// 
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // 加载 API 接口描述文档
            GlobalConfiguration.Configure(ApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // 使用自定义的 版本 控制路由定位到制定版本的 Controller
            ControllerBuilder.Current.SetControllerFactory(typeof(VersionControllerFactory));
        }
    }
}
