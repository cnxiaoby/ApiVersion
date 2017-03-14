using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace ApiVersion.Api
{
    /// <summary>
    /// 控制器类型集合工具类
    /// </summary>
    public class ControllerTypeDictionarys
    {
        private static Dictionary<string, Type> _controllerDictionary = null;

        /// <summary>
        /// 控制器类型集合
        /// </summary>
        public static Dictionary<string, Type> Values
        {
            get
            {
                if (_controllerDictionary == null)
                {
                    var dictionary = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

                    Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
                    List<Type> controllerTypes = new List<Type>();
                    foreach (Type controller in allTypes)
                    {
                        if (!string.IsNullOrEmpty(controller.FullName) &&
                            System.Text.RegularExpressions.Regex.IsMatch(controller.FullName, ApiConfig.API_NAMESPACE_REGEXTEXT))
                        {
                            controllerTypes.Add(controller);
                        }
                    }

                    // 获取当前程序集
                    foreach (Type controller in controllerTypes)
                    {
                        var namespaceArray = controller.FullName.Split(Type.Delimiter);
                        if (namespaceArray.Length >= 3)
                        {
                            string controllerName = controller.Name;
                            string version = namespaceArray[namespaceArray.Length - 3];
                            string key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version, controllerName);

                            if (!dictionary.Keys.Contains(key))
                            {
                                dictionary[key] = controller;
                            }
                        }
                    }
                    _controllerDictionary = dictionary;
                }

                return _controllerDictionary;
            }
        }

        private const string VersionKey = "version";
        private const string ControllerKey = "controller";
        /// <summary>
        /// 从 Url 中获取版本，如：your.com/v1/controller/action/parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routeData"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static T GetRouteVariable<T>(RouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }
        /// <summary>
        /// 从 Header 中获取版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetVersionFromHTTPHeaderAndAcceptHeader(HttpRequestBase request)
        {
            if (request.Headers.AllKeys.Contains(VersionKey))
            {
                var versionHeader = request.Headers.GetValues(VersionKey).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据 RequestContext 和 ControllerName 获取控制器的类型 Type
        /// 判断 version
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            RouteData routeData = requestContext.RouteData;
            if (routeData == null)
            {
                throw new Exception("NotFound");
            }

            string version = GetRouteVariable<string>(routeData, VersionKey);
            if (string.IsNullOrEmpty(version))
            {
                version = GetVersionFromHTTPHeaderAndAcceptHeader(requestContext.HttpContext.Request);
            }
            if (string.IsNullOrEmpty(version))
            {
                version = "Base";
            }

            version = version.Replace(".", "_");

            string key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}Controller", version, controllerName);
            if (!Values.ContainsKey(key))
            {
                key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}Controller", "Base", controllerName);
            }

            Type controllerType;
            if (Values.TryGetValue(key, out controllerType))
            {
                return controllerType;
            }
            else
            {
                throw new Exception(key + " NotFound");
            }
        }
    }
}