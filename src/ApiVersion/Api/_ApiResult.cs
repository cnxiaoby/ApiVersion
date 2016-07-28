using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api
{
    public class ApiResult : ActionResult
    {
        public ApiResult() { }
        public ApiResult(object data)
        {
            this.errcode = 0;
            this.data = data;
        }
        public ApiResult(int code, object data)
        {
            this.errcode = code;
            this.data = data;
        }

        int _errcode = -1;
        /// <summary>
        /// 状态码，默认为 -1 ,0为请求成功
        /// </summary>
        public int errcode
        {
            get { return _errcode; }
            set { _errcode = value; }
        }

        string _errmsg = string.Empty;
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string errmsg
        {
            get { return _errmsg; }
            set { _errmsg = value; }
        }

        object _data = null;
        /// <summary>
        /// 返回的数据内容
        /// </summary>
        public object data
        {
            get { return _data; }
            set { _data = value; }
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            //var result = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            response.ContentType = "application/json; charset=utf-8";
            response.Write(this);
        }
    }
}