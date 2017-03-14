using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace ApiVersion
{
    /// <summary>
    /// 加载 API 描述
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// API 描述文档
        /// </summary>
        public const string DOCUMENT_FILE_PATH = "~/bin/ApiVersion.XML";
        /// <summary>
        /// 需要控制的命名空间正则表达式
        /// </summary>
        public const string API_NAMESPACE_REGEXTEXT = @"^ApiVersion\.Api\.(\w+)\.Controllers\.(\w+)$";

        private static readonly int ApiNamespace_PrefixLength = ("ApiVersion.Api.").Length;

        /// <summary>
        /// 注册 API 接口描述文档
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            var showlist = config.Services.GetApiExplorer().ApiDescriptions;
            // 查找 API Type
            Type[] allTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            List<Type> controllerTypes = new List<Type>();
            foreach (Type controller in allTypes)
            {
                if (!string.IsNullOrEmpty(controller.FullName) && Regex.IsMatch(controller.FullName, API_NAMESPACE_REGEXTEXT))
                {
                    controllerTypes.Add(controller);
                }
            }

            var DocumentationProvider = config.Services.GetDocumentationProvider();
            foreach (Type controller in controllerTypes)
            {
                var version = controller.FullName.Substring(ApiNamespace_PrefixLength);
                version = version.Substring(0, version.IndexOf("."));
                if (version == "Base")
                    version = string.Empty;
                else
                    version += "/";

                var controllerName = controller.Name;
                if (controllerName.EndsWith("Controller"))
                {
                    int len = controllerName.LastIndexOf("Controller");
                    controllerName = controllerName.Substring(0, len);
                }

                HttpControllerDescriptor controllerDescriptor = new HttpControllerDescriptor(config, controllerName, controller);

                var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    string actionName = method.Name;
                    ApiDescription api = new ApiDescription();

                    // ActionDescriptor
                    api.ActionDescriptor = new ReflectedHttpActionDescriptor(controllerDescriptor, method);
                    api.Documentation = DocumentationProvider.GetDocumentation(api.ActionDescriptor);

                    // 使用反射设置 ResponseDescription 
                    Type apiDescriptionType = typeof(ApiDescription);
                    foreach (PropertyInfo proInfo in apiDescriptionType.GetProperties())
                    {
                        if (proInfo.Name == "ResponseDescription")
                        {
                            if (proInfo.CanWrite)
                                proInfo.SetValue(api, new ResponseDescription(), null);
                            break;
                        }
                    }

                    api.ResponseDescription.ResponseType = method.ReturnType;
                    api.ResponseDescription.Documentation = DocumentationProvider.GetResponseDocumentation(api.ActionDescriptor);


                    // GET or POST
                    var getOrPost = method.GetCustomAttributes(typeof(System.Web.Mvc.HttpPostAttribute), false);
                    api.HttpMethod = getOrPost.Length > 0 ? HttpMethod.Post : HttpMethod.Get;

                    // Requst Url
                    var routeAtts = method.GetCustomAttributes(typeof(RouteAttribute), false);
                    if (routeAtts.Length > 0)
                    {
                        var routeTemp = (RouteAttribute)routeAtts[0];
                        api.RelativePath = routeTemp.Template;
                    }
                    else
                    {
                        api.RelativePath = string.Format("{0}{1}/{2}", version, controllerName, actionName);
                        if (api.HttpMethod == HttpMethod.Get)
                        {
                            var sb = new StringBuilder();
                            foreach (ParameterInfo t in method.GetParameters())
                            {
                                sb.AppendLine("&" + t.Name + "={" + t.Name + "}");
                            }
                            if (sb.Length > 0)
                                api.RelativePath += "?" + sb.ToString().Trim('&');
                        }
                    }

                    // ParameterDescriptions
                    foreach (ParameterInfo t in method.GetParameters())
                    {
                        var item = new ApiParameterDescription();
                        item.Name = t.Name;
                        item.ParameterDescriptor = new ReflectedHttpParameterDescriptor(api.ActionDescriptor, t);
                        item.Documentation = DocumentationProvider.GetDocumentation(item.ParameterDescriptor);
                        item.Documentation = ConvertRN(item.Documentation, "<br />");
                        item.Source = api.HttpMethod == HttpMethod.Get ? ApiParameterSource.FromUri : ApiParameterSource.FromBody;
                        api.ParameterDescriptions.Add(item);
                    }

                    //api.SupportedRequestBodyFormatters.Add(new FormUrlEncodedMediaTypeFormatter());
                    api.SupportedResponseFormatters.Add(new JsonMediaTypeFormatter());

                    /*
                     des.GetFriendlyId()
                     des.HttpMethod.Method 
                     des.RelativePath
                     des.Documentation
                    */
                    //des.Route = new System.Web.Routing.Route("");
                    //des.ParameterDescriptions.Add(new ApiParameterDescription());

                    showlist.Add(api);
                }
            }
        }

        /// <summary>
        /// 换行符替换为给定字符串
        /// </summary>
        /// <param name="src">要清除的字符串</param>
        /// <param name="replace">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ConvertRN(string src, string replace)
        {
            if (string.IsNullOrEmpty(src))
                return string.Empty;

            System.Text.RegularExpressions.Match m = null;
            Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            for (m = RegexBr.Match(src); m.Success; m = m.NextMatch())
            {
                src = src.Replace(m.Groups[0].ToString(), replace);
            }
            return src;
        }

    }
}