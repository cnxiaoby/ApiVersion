using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace ApiVersion.Api
{
    /// <summary>
    /// 版本控制器工厂类
    /// </summary>
    public class VersionControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 根据 RequestContext 和 ControllerName 获取控制器的类型 Type
        /// 判断 version , 调用不同版本的 Controller 实现
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            // 判断是否文档描述 Areas 请求
            if (controllerName.ToLower().Equals("help"))
                return base.GetControllerType(requestContext, controllerName);
            else
                return ControllerTypeDictionarys.GetControllerType(requestContext, controllerName);
        }
    }
}