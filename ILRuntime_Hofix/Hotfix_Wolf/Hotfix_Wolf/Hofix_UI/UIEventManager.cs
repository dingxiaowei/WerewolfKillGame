using System;

namespace Hotfix.UI
{
    /// <summary>
    /// 事件
    /// </summary>
    public class UIEventManager
    {
        /// <summary>
        /// 显示界面
        /// </summary>
        public static event Action<int, object, Action> showViewAction;
        /// <summary>
        /// 显示界面调用
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="data"></param>
        public static void ShowView(int viewId, object data = null, Action callBack = null)
        {
            showViewAction?.Invoke(viewId, data, callBack);
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        public static event Action<int, object> refreshViewDataAction;
        /// <summary>
        /// 刷新事件调用
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="data"></param>
        public static void RefreshView(int viewId, object data)
        {
            refreshViewDataAction?.Invoke(viewId, data);
        }
        public static void EventSubscribe()
        {

        }
        public static void EventRemove()
        {

        }
    }
}
