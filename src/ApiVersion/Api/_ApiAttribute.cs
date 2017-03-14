using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiVersion.Api
{
    /// <summary>
    /// Api 特性描述
    /// </summary>
    public class ApiAttribute : Attribute
    {
        /// <summary>
        /// 禁用授权控制，默认是 false
        /// </summary>
        public bool DisableAuthorize { get; set; }
        /// <summary>
        /// 失效
        /// </summary>
        public bool Invalidation { get; set; }
    }
}