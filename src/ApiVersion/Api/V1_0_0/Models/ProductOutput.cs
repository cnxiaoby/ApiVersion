using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiVersion.Api.V1_0_0.Models
{
    /// <summary>
    /// 产品
    /// </summary>
    public class ProductOutput: Base.Models.ProductOutput
    {
        /// <summary>
        /// 产品图片
        /// </summary>
        public string img { get; set; }        
    }
    
}