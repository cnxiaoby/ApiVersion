using ApiVersion.Api.V1_0_0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api.V1_0_0.Controllers
{
    [Api(DisableAuthorize = true)]
    public class DefaultController : Base.Controllers.DefaultController
    {
        public override ApiResult Index()
        {
            return Success(new { name= "这里是 V1.0.0 API" });
        }

        [Api(Invalidation = true)]
        public override ApiResult Old()
        {
            return Success(new { name = "这是过期的API，客户端访问不到，只有父类才可以访问" });
        }

        public override ApiResult Prodeuct(int id)
        {
            ApiResult result = base.Prodeuct(id);
            Base.Models.ProductOutput outputOld = result.data as Base.Models.ProductOutput;

            V1_0_0.Models.ProductOutput output = outputOld.ConvertTo<V1_0_0.Models.ProductOutput>();

            output.img = "图片";

            return Success(output);
        }
    }
}