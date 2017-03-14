using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiVersion.Api
{
    /// <summary>
    /// Api.code 的解析码
    /// </summary>
    public class ApiCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const int SUCCESS = 0;
        /// <summary>
        /// 需要用户登录的 token
        /// </summary>
        public const int NEED_TOKEN = 5001;
        /// <summary>
        /// 没有找到相关信息
        /// </summary>
        public const int NULL_REQUST_INFO = 5002;
    }
}