using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiVersion;
using ApiVersion.Api.Base.Models;

namespace ApiVersion.Api.Base.Controllers
{
    [Api(DisableAuthorize = true)]
    public class DefaultController : BaseController
    {
        public virtual ApiResult Index()
        {
            return Success(new { name = "这里是 Base API Index" });
        }

        public virtual ApiResult Old()
        {
            return Success(new { name = "这里是 Base API Old" });
        }

        public virtual ApiResult Prodeuct(int id)
        {
            ProductOutput output = new ProductOutput();
            output.id = id;
            output.name = "产品" + id;
            output.categoryName = "分类";

            return Success(output);
        }
    }
}