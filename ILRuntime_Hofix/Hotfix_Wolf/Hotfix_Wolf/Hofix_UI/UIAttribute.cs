namespace Hotfix.UI
{
    /// <summary>
    /// UI界面自定义属性 UI的Id UI的加载路径
    /// </summary>
    public class UIAttribute : ManagerAttribute
    {
        public string resourcePath { get; private set; }
        public UIAttribute(ViewId id, string value) : base(id.ToString())
        {
            this.resourcePath = value;
        }
    }
}
