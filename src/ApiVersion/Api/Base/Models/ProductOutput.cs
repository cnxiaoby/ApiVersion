using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiVersion.Api.Base.Models
{
    /// <summary>
    /// 产品
    /// </summary>
    public class ProductOutput: BaseModel
    {
        /// <summary>
        /// 产品 id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string name { get; set; }        
        /// <summary>
        /// 分类名称
        /// </summary>
        public string categoryName { get; set; }        
    }
    
}