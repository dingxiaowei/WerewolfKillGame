using System.Collections.Generic;

namespace Hotfix.UI
{
    /// <summary>
    /// 存储数据
    /// </summary>
    public class UIModel
    {
        public static UIModel m_Instance;
        public static UIModel Instance
        {
            get
            {
                return m_Instance ?? (m_Instance = new UIModel());
            }
        }
        /// <summary>
        /// 存储数据 字典
        /// </summary>
        Dictionary<ViewId, object> m_ViewDataDict = new Dictionary<ViewId, object>();
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="getData"></param>
        public void SetData(ViewId viewId, object getData)
        {
            m_ViewDataDict[viewId] = getData;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public object GetData(ViewId viewId, object defaultData = null)
        {
            if (m_ViewDataDict.Count == 0)
            {
                return null;
            }
            if (m_ViewDataDict.ContainsKey(viewId))
            {
                return m_ViewDataDict[viewId] ?? defaultData;
            }
            return null;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="getData"></param>
        public void RemoveData(ViewId viewId)
        {
            if (m_ViewDataDict.ContainsKey(viewId))
            {
                m_ViewDataDict.Remove(viewId);
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void ClearData()
        {
            m_ViewDataDict.Clear();
        }
    }
}
