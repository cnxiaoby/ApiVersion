using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api.V1_1_0.Controllers
{
    /// <summary>
    /// 继承 Base Default 业务的接口 v1.1.0
    /// </summary>
    [Api(DisableAuthorize = true)]
    public class DefaultController : Base.Controllers.DefaultController
    {
        /// <summary>
        /// 业务 Index
        /// </summary>
        /// <returns></returns>
        [VersionActionSelector]
        public new ApiResult Index()
        {
            return Success(new { name = "这里是 v1.1.0 API Index" });
        }

        /// <summary>
        /// 业务 Test
        /// </summary>
        /// <returns></returns>
        [VersionActionSelector]
        public new ApiResult Test()
        {
            return Error(-1, "Test 是过期的API，客户端访问不到，只有上一版本才可以访问");
        }
    }
}