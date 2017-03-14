using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ApiVersion.Api
{
    /// <summary>
    /// 分页
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 页码，第几页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 页的记录大小，每页多少条记录
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 当前查询的总记录数
        /// </summary>
        public int totalSize { get; set; }
    }

    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class ApiResult : ActionResult
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ApiResult() { }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(object data)
        {
            this.code = 0;
            this.data = data;
        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public ApiResult(int code, object data)
        {
            this.code = code;
            this.data = data;
        }

        int _code = -1;
        /// <summary>
        /// 状态码，默认为 -1 ,0为请求成功
        /// </summary>
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }

        string _msg = string.Empty;
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
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
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 输出处理
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json; charset=utf-8";

            // 跨域方法一：添加 header
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "x-requested-with");

            var result = context.HttpContext.Request;
            string callback = string.Empty;
            if (result.QueryString["callback"] != null)
            {
                // 跨域方法二：jsonp 支持
                callback = result.QueryString["callback"];
                response.Write(string.Concat(callback, "(", this.ToString(), ")"));
            }
            else
            {
                response.Write(this);
            }
        }
    }
    /// <summary>
    /// 分页结果
    /// </summary>
    public class ApiListResult : ApiResult
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public Pagination paging { get; set; }
    }

    /// <summary>
    /// 泛型返回结果对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiListResult<T> : ApiResult<T>
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public Pagination paging { get; set; }
    }
    /// <summary>
    /// 泛型返回结果对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ActionResult
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ApiResult() { }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(T data)
        {
            this.code = 0;
            this.data = data;
        }
        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public ApiResult(int code, T data)
        {
            this.code = code;
            this.data = data;
        }

        int _code = -1;
        /// <summary>
        /// 状态码，默认为 -1 ,0为请求成功
        /// </summary>
        public int code
        {
            get { return _code; }
            set { _code = value; }
        }

        string _msg = string.Empty;
        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        T _data = default(T);
        /// <summary>
        /// 返回的数据内容
        /// </summary>
        public T data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 输出处理
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json; charset=utf-8";

            // 跨域方法一：添加 header
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "x-requested-with");

            var result = context.HttpContext.Request;
            string callback = string.Empty;
            if (result.QueryString["callback"] != null)
            {
                // 跨域方法二：jsonp 支持
                callback = result.QueryString["callback"];
                response.Write(string.Concat(callback, "(", this.ToString(), ")"));
            }
            else
            {
                response.Write(this);
            }
        }
    }
}