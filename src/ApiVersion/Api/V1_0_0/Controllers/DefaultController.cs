using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api.V1_0_0.Controllers
{
    /// <summary>
    /// 继承 Base Default 业务的接口 v1.0.0
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
            return Success(new { name = "这里是 v1.0.0 API Index" });
        }

        /// <summary>
        /// 业务 Test
        /// </summary>
        /// <returns></returns>
        [Api(Invalidation = true)]
        [VersionActionSelector]
        public new ApiResult Test()
        {
            return Success(new { name = "这里是 v1.0.0 API Test" });
        }

        /// <summary>
        /// 获取制定产品信息
        /// </summary>
        /// <param name="id">产品id</param>
        /// <returns></returns>
        [VersionActionSelector]
        public new ApiResult<V1_0_0.Models.ProductOutput> Prodeuct(int id)
        {
            var output = new V1_0_0.Models.ProductOutput();
            output.id = id;
            output.name = "v1.0.0 产品" + id;
            output.categoryName = "分类";
            output.img = "http://www.test.com/img.png";

            return SuccessFor(output);
        }
    }
}