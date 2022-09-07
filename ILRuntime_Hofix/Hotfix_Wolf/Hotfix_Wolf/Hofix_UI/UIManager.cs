using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hotfix.UI
{
    /// <summary>
    /// UI层级
    /// </summary>
    public enum UILayer
    {
        Bottom,
        Center,
        Top,
    }
    /// <summary>
    /// 界面状态
    /// </summary>
    public enum ViewStatus
    {
        Show = 1,
        Hide,
        Destroy
    }
    /// <summary>
    /// 管理界面、数据
    /// </summary>
    public class UIManager : ManagerBaseWithAttr<UIManager, UIAttribute>
    {
        /// <summary>
        /// 测试 界面根节点
        /// </summary>
        private Transform uiRoot;
        /// <summary>
        /// 设置层级节点
        /// </summary>
        private Transform Bottom, Center, Top;
        /// <summary>
        /// 缓存当前场景的生成界面
        /// </summary>
        private Dictionary<ViewId, UIBase> m_ViewsDict;
        /// <summary>
        /// 界面 栈
        /// </summary>
        private Stack<UIBase> m_ViewsStack;
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            m_ViewsDict = new Dictionary<ViewId, UIBase>();
            m_ViewsStack = new Stack<UIBase>();

            uiRoot = GameObject.Find("UI/UIRoot/View Root").transform;

            Bottom = GameObject.Find("UI/UIRoot/Bottom Root").transform;
            Center = GameObject.Find("UI/UIRoot/Center Root").transform;
            Top = GameObject.Find("UI/UIRoot/Top Root").transform;

            Debug.Log("UIManager 初始化");
            ///事件监听
            EventSubscribe();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="viewId"></param> UI id
        /// <returns></returns>
        public UIBase CreateView(ViewId viewId)
        {
            var data = this.GetAttributeData(viewId.ToString());
            if (data == null)
            {
                Debug.LogError("无法加载");
                return null;
            }
            var attr = data.attribute as UIAttribute;
            var view = Activator.CreateInstance(data.type, new object[] { attr.resourcePath }) as UIBase;
            return view;
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="viewId"></param> 界面ID
        /// <param name="data"></param> 数据信息
        /// <param name="callback"></param> 回调
        ///params int[] viewsId
        public void ShowView(ViewId viewId, object data = null, Action callback = null)
        {
            //存储数据
            UIModel.Instance.SetData(viewId, data);

            UIBase curView;
            if (m_ViewsDict.ContainsKey(viewId))
            {
                curView = m_ViewsDict[viewId];
            }
            else
            {
                curView = CreateView(viewId);
                curView.Load(callback);
                curView.InitPrefab((layer) =>
                {
                    return GetLayerParent(layer);
                });
                m_ViewsDict[viewId] = curView;
            }
            curView.transform.SetAsLastSibling();
            curView.Show();
            if (data != null)
            {
                curView.RefreshShowInfo(data);
            }
            //界面栈
            AddViewToStack(curView);
        }
        /// <summary>
        /// 父对象
        /// </summary>
        /// <param name="layer"></param> 对应的层级
        /// <returns></returns>
        private Transform GetLayerParent(UILayer layer)
        {
            var curRoot = Bottom;
            switch (layer)
            {
                case UILayer.Bottom:
                    curRoot = Bottom;
                    break;
                case UILayer.Center:
                    curRoot = Center;
                    break;
                case UILayer.Top:
                    curRoot = Top;
                    break;
                default:
                    curRoot = Bottom;
                    break;
            }
            return curRoot;
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="viewId"></param> UI id
        public void HideView(ViewId viewId)
        {
            if (m_ViewsDict.ContainsKey(viewId))
            {
                var curView = m_ViewsDict[viewId];
                curView.Hide();

                RemoveViewFromStack(curView);
                ShowViewFromStack();
            }
            else
            {
                Debug.Log("没有可隐藏的界面");
            }
        }
        /// <summary>
        /// 销毁单个
        /// </summary>
        /// <param name="viewId"></param>
        public void DestroyView(ViewId viewId)
        {
            if (m_ViewsDict.ContainsKey(viewId))
            {
                var curView = m_ViewsDict[viewId];
                curView.Destroy();

                RemoveViewFromStack(curView);
                ShowViewFromStack();
                //数据移除
                m_ViewsDict.Remove(viewId);
                UIModel.Instance.RemoveData(viewId);
            }
            else
            {
                Debug.LogError("没有可銷毀的界面");
            }
        }
        /// <summary>
        /// 销毁全部
        /// </summary>
        public void DestroyAll()
        {
            foreach (var view in m_ViewsDict)
            {
                view.Value.Destroy();
            }
            m_ViewsDict.Clear();

            UIModel.Instance.ClearData();
        }

        /// <summary>
        /// 栈里添加
        /// </summary>
        /// <param name="curView"></param> 当前UI
        private void AddViewToStack(UIBase curView)
        {
            if (m_ViewsStack.Count > 0)
            {
                var getPopView = m_ViewsStack.Peek();
                if (getPopView != null && getPopView == curView)
                    return;
            }

            m_ViewsStack.Push(curView);
        }

        /// <summary>
        /// 栈里移出
        /// </summary>
        private void RemoveViewFromStack(UIBase curView)
        {
            var stackNum = m_ViewsStack.Count;
            var lastIndex = stackNum - 1;
            if (m_ViewsStack.ElementAt(lastIndex) == curView)
            {
                m_ViewsStack.Pop();
            }
            else
            {
                var stackTmp = new Stack<UIBase>();
                for (var i = 0; i < stackNum; i++)
                {
                    var viewTmp = m_ViewsStack.ElementAt(i);
                    if (viewTmp != curView)
                    {
                        stackTmp.Push(viewTmp);
                    }
                }
                m_ViewsStack = stackTmp;
            }
        }

        /// <summary>
        /// 展示上一个界面
        /// </summary>
        private void ShowViewFromStack()
        {
            if (m_ViewsStack.Count == 0)
            {
                return;
            }
            var lastView = m_ViewsStack.Peek();
            if (lastView != null && lastView.isLoaded)
            {
                lastView.Show();
            }
        }
        /// <summary>
        /// 获取当前的View
        /// </summary>
        /// <param name="viewId"></param> UI id
        public T GetView<T>(ViewId viewId) where T : UIBase
        {
            if (m_ViewsDict.ContainsKey(viewId))
            {
                var curView = m_ViewsDict[viewId];
                return curView as T;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取界面
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public UIBase GetView(ViewId viewId)
        {
            if (m_ViewsDict.ContainsKey(viewId))
            {
                return m_ViewsDict[viewId];
            }
            return null;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="viewId"></param> UI id
        /// <returns></returns>
        public object GetViewData(ViewId viewId)
        {
            return UIModel.Instance.GetData(viewId);
        }
        /// <summary>
        /// 事件添加
        /// </summary>
        private void EventSubscribe()
        {
            UIEventManager.showViewAction += LoadShowView;
            UIEventManager.refreshViewDataAction += RefreshViewData;
            UIEventManager.EventSubscribe();
        }
        /// <summary>
        /// 事件移除
        /// </summary>
        private void EventRemove()
        {
            UIEventManager.showViewAction -= LoadShowView;
            UIEventManager.refreshViewDataAction -= RefreshViewData;
            UIEventManager.EventRemove();
        }
        ///调用 UIEventManager.ShowView(viewId, data, callBack);
        /// <summary>
        /// 界面显示
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="data"></param>
        private void LoadShowView(int viewId, object data, Action callBack)
        {
            ShowView((ViewId)viewId, data, callBack);
        }
        ///调用 UIEventManager.RefreshView(viewId, data);
        /// <summary>
        /// 界面刷新
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="data"></param>
        private void RefreshViewData(int viewId, object data)
        {
            UIModel.Instance.SetData((ViewId)viewId, data);

            var curView = m_ViewsDict[(ViewId)viewId];
            if (curView != null)
            {
                if (data != null)
                {
                    curView.RefreshShowInfo(data);
                }
            }
        }
    }
}
