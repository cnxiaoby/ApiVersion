using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api
{
    /// <summary>
    /// 高级版本继承低版本的API，同名方法覆盖(new)，需要使用特性 [VersionActionNameSelector]
    /// </summary>
    public class VersionActionSelectorAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// 确定操作方法选择对指定的控制器上下文是否有效
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var type = controllerContext.Controller.GetType();
            return methodInfo.DeclaringType.Equals(type);
        }
    }
}