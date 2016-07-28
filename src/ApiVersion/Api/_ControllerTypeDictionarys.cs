using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace ApiVersion.Api
{
    public class ControllerTypeDictionarys
    {
        private static Dictionary<string, Type> _controllerDictionary;
        public static Dictionary<string,Type> Values
        {
            get
            {
                if(_controllerDictionary == null)
                {
                    var dictionary = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

                    // 获取当前程序集
                    Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
                    List<Type> controllerTypes = new List<Type>();
                    foreach (Type controller in allTypes)
                    {
                        // 匹配 ApiVersion.Api.{0}.Controllers.{1} 的Type
                        // 找到所有的 Controller
                        if (!string.IsNullOrEmpty(controller.FullName) &&
                            System.Text.RegularExpressions.Regex.IsMatch(controller.FullName,
                            @"^ApiVersion\.Api\.(\w+)\.Controllers\.(\w+)$"))
                        {
                            controllerTypes.Add(controller);
                        }
                    }

                    foreach (Type controller in controllerTypes)
                    {
                        var namespaceArray = controller.FullName.Split(Type.Delimiter);
                        if (namespaceArray.Length >= 3)
                        {
                            string controllerName = controller.Name;
                            string version = namespaceArray[namespaceArray.Length - 3];
                            // key 的命名规则为  {version}.{controller} 如：Base.DefaultController 、 V1_0_0.DefaultController
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
        private static T GetRouteVariable<T>(RouteData routeData, string name)
        {
            object result = null;
            if (routeData.Values.TryGetValue(name, out result))
            {
                return (T)result;
            }
            return default(T);
        }

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
                // 向上检索旧版本是否有
                key = String.Format(CultureInfo.InvariantCulture, "{0}.{1}Controller", "Base", controllerName);
            }

            Type controllerType;
            if (Values.TryGetValue(key, out controllerType))
            {
                return controllerType;
            }
            else
            {
                throw new Exception("NotFound");
            }
        }
    }
}