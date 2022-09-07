using System;

namespace Hotfix.UI
{
    /// <summary>
    /// 属性接口
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 检查Type
        /// </summary>
        /// <param name="type"></param>
        void CheckType(Type type);
        /// <summary>
        /// 生成类的实例
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        T2 CreateInstance<T2>(string name, params object[] args) where T2 : class;
        /// <summary>
        /// 获取AttributeData
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        AttributeData GetAttributeData(string name);
    }
}
