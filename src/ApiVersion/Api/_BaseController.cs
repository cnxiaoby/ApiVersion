using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api
{
    /// <summary>
    /// 这里定义为 abstract 类，是为了只用于继承，不可以直接使用
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 请求错误
        /// </summary>
        /// <returns></returns>
        public ApiResult Error(int code, string msg)
        {
            ApiResult result = new ApiResult();
            result.errcode = code;
            result.errmsg = msg;
            return result;
        }

        /// <summary>
        /// 授权失败
        /// </summary>
        /// <returns></returns>
        public ApiResult ErrorAuthorizeFailure()
        {
            return Error(40001, "请求认证失败/Authorize failur");
        }

        /// <summary>
        /// API 失效
        /// </summary>
        /// <returns></returns>
        public ApiResult ErrorApiInvalidation()
        {
            return Error(40002, "API 失效/API Invalidation");
        }

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ApiResult Success(object data)
        {
            ApiResult result = new ApiResult();
            result.errcode = 0;
            result.data = data;
            return result;
        }
        public ApiResult Success()
        {
            return Success(new object());
        }
        /// <summary>
        /// 请求到达，首先进行授权判断
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 输出API帮助信息
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            if (Request.RawUrl.EndsWith(controllerName + "/" + actionName + "?help", StringComparison.OrdinalIgnoreCase))
            {
                Response.Write("这里是 "+ controllerName + "/" + actionName + " 帮助信息");
                Response.End();
            }

            bool disableAuthorization = false;
            bool invalidation = false;
            #region 读取 Action 和 Controller 的 ApiAttribute 看看是否被描述为不执行授权判断

            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ApiAttribute), false);
            if (attrs == null || attrs.Length == 0)
            {
                attrs = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ApiAttribute), false);
            }

            // 若有描述，则判断是否不执行授权判断
            foreach(object attrobj in attrs)
            {
                ApiAttribute auth = attrobj as ApiAttribute;
                if (!disableAuthorization && auth.DisableAuthorize)
                {
                    disableAuthorization = true;
                }
                if (!invalidation && auth.Invalidation)
                {
                    invalidation = true;
                }
            }

            #endregion
            // 进行授权判断
            if (disableAuthorization == false)
            {
                string authorization = this.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorization) || !AuthorizationKey(authorization))
                {
                    filterContext.Result = ErrorAuthorizeFailure();
                }
            }
            // 进行失效判断
            if (invalidation == true)
            {
                filterContext.Result = ErrorApiInvalidation();
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 授权验证，返回 True 为通过，False 为授权失败
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        private bool AuthorizationKey(string authorization)
        {
            // 
            // TODO:
            // 这里编写您的授权验证逻辑
            //

            // Test：
            return authorization == "123456";
        }
    }
}