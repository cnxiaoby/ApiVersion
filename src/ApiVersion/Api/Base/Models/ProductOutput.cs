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
        public int id { get; set; }
        public string name { get; set; }        
        public string categoryName { get; set; }        
    }
    
}