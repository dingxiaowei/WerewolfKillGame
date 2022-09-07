using System;
using System.Collections.Generic;
using UnityEngine;
using Hotfix.Manager;

namespace Hotfix.UI
{
    /// <summary>
    /// 属性详情
    /// </summary>
    public class AttributeData
    {
        public ManagerAttribute attribute;
        public Type type;
    }
    /// <summary>
    /// 自定义属性
    /// </summary>
    public class ManagerAttribute : Attribute
    {
        public string value { get; private set; }
        public ManagerAttribute(string value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// 管理UI界面属性信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class ManagerBaseWithAttr<T, V> : ManagerBase<T>, IAttribute where T : IManager, new() where V : ManagerAttribute
    {
        protected Dictionary<string, AttributeData> m_AttributeDataDict;
        protected ManagerBaseWithAttr()
        {
            this.m_AttributeDataDict = new Dictionary<string, AttributeData>();
        }
        /// <summary>
        /// 检测自定义界面
        /// </summary>
        /// <param name="type"></param>
        public virtual void CheckType(Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(V), false);
            if (attrs.Length > 0)
            {
                var attr = attrs[0];
                if (attr is V)
                {
                    //Debug.LogError($"attr is {attr}");
                    var _attr = (V)attr;
                    SaveAttribute(_attr.value, new AttributeData() { attribute = _attr, type = type });
                }
            }
        }
        /// <summary>
        /// 保存界面属性数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SaveAttribute(string name, AttributeData data)
        {
            this.m_AttributeDataDict[name] = data;
        }
        /// <summary>
        /// 获取界面属性数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AttributeData GetAttributeData(string name)
        {
            AttributeData data = null;
            this.m_AttributeDataDict.TryGetValue(name, out data);
            return data;
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T2 CreateInstance<T2>(string name, params object[] args) where T2 : class
        {
            var data = GetAttributeData(name).type;
            if (data == null)
            {
                Debug.LogError($"没有找到{name}-{typeof(T2).Name}");
                return null;
            }
            else
            {
                if (args.Length == 0)
                {
                    return Activator.CreateInstance(data) as T2;
                }
                else
                {
                    return Activator.CreateInstance(data, args) as T2;
                }
            }

        }
    }
}
