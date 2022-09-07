using System;
using UnityEngine;

namespace Hotfix.UI
{
    /// <summary>
    /// UI的基类 
    /// </summary>
    public class UIBase : IView
    {
        /// <summary>
        /// 预设路径
        /// </summary>
        public string resourcePath;
        /// <summary>
        /// 预设层级
        /// </summary>
        public UILayer layer = UILayer.Bottom;
        public GameObject gameObject;
        public Transform transform;
        public RectTransform rectTransform;
        /// <summary>
        /// 是否加载
        /// </summary>
        public bool isLoaded
        {
            get
            {
                return (gameObject != null);
            }
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isVisible
        {
            get
            {
                return isLoaded && gameObject.activeSelf;
            }
            set
            {
                if (isLoaded)
                    gameObject.SetActive(value);
            }
        }
        public UIBase(string resourcePath)
        {
            this.resourcePath = resourcePath;
        }
        /// <summary>
        /// UI 初始化
        /// </summary>
        public virtual void Init()
        {
            isVisible = false;
        }
        /// <summary>
        /// UI 显示
        /// </summary>
        public virtual void Show()
        {
            isVisible = true;
        }
        /// <summary>
        /// UI 隐藏
        /// </summary>
        public virtual void Hide()
        {
            isVisible = false;
        }
        /// <summary>
        /// UI 销毁
        /// </summary>
        public virtual void Destroy()
        {
            GameObject.DestroyImmediate(gameObject);
            gameObject = null;
            transform = null;
            rectTransform = null;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public virtual void Update()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public virtual void FixedUpdate()
        {

        }
        /// <summary>
        /// 加载界面预设
        /// </summary>
        /// <param name="callBack"></param> 加载成功的回调
        public virtual void Load(Action callBack = null)
        {
            gameObject = GameObject.Instantiate(Resources.Load(resourcePath)) as GameObject;
            if (gameObject != null)
            {
                transform = gameObject.transform;
                rectTransform = transform.GetComponent<RectTransform>();

                //自动查找节点
                UITools.AutoSetTransformPath(this);

                Init();
                callBack?.Invoke();
            }
            else
            {
                Debug.LogError($"{resourcePath}界面为 Null");
            }
        }
        /// <summary>
        /// 设置界面父对象 初始化位置和缩放
        /// </summary>
        /// <param name="getLayer"></param> 获取界面所在的层级
        public virtual void InitPrefab(Func<UILayer, Transform> getLayer)
        {
            transform.SetParent(getLayer(layer));
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(0, 0);
        }
        /// <summary>
        /// 刷新界面需要的数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void RefreshShowInfo(object data)
        {

        }
    }
}
