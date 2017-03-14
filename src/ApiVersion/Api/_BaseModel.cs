using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ApiVersion.Api
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseModel
    {
        /// <summary>
        /// 将对象的属性值赋值给为具有相同属性名称的其它对象
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel ConvertTo<TModel>()
        {
            TModel model = Activator.CreateInstance<TModel>();
            Type modelType = typeof(TModel);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                PropertyInfo property = modelType.GetProperty(descriptor.Name);
                try
                {
                    if (property != null && property.CanWrite)
                    {
                        object value = descriptor.GetValue(this);
                        if (!Convert.IsDBNull(value) && value != null)
                            property.SetValue(model, value, null);
                    }
                }
                catch { }
            }
            return model;
        }
    }
}