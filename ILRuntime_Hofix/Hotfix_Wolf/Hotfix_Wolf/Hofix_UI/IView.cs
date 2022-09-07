namespace Hotfix.UI
{
    /// <summary>
    /// UI接口
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 显示
        /// </summary>
        void Show();
        /// <summary>
        /// 隐藏
        /// </summary>
        void Hide();
        /// <summary>
        /// 销毁
        /// </summary>
        void Destroy();
        /// <summary>
        /// 刷新
        /// </summary>
        void Update();
        void LateUpdate();
        void FixedUpdate();
    }
}
