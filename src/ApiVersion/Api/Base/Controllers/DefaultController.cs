using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiVersion;
using ApiVersion.Api.Base.Models;

namespace ApiVersion.Api.Base.Controllers
{
    /// <summary>
    /// 某业务接口
    /// </summary>
    [Api(DisableAuthorize = true)]
    public class DefaultController : BaseController
    {
        /// <summary>
        /// 业务 Index
        /// </summary>
        /// <returns></returns>
        public ApiResult Index()
        {
            return Success(new { name = "这里是 Base API Index" });
        }
        /// <summary>
        /// 业务 Test
        /// </summary>
        /// <returns></returns>
        public ApiResult Test()
        {
            return Success(new { name = "这里是 Base API Test" });
        }
        /// <summary>
        /// 获取制定产品信息
        /// </summary>
        /// <param name="id">产品id</param>
        /// <returns></returns>
        public ApiResult<ProductOutput> Prodeuct(int id)
        {
            ProductOutput output = new ProductOutput();
            output.id = id;
            output.name = "Base 产品" + id;
            output.categoryName = "分类";

            return SuccessFor(output);
        }
    }
}