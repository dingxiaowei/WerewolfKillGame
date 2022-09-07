using System;

namespace Hotfix.UI
{
    /// <summary>
    ///  寻找节点的自定义属性
    /// </summary>
    public class TransformPath : Attribute
    {
        public string Path;

        public TransformPath(string path)
        {
            this.Path = path;
        }
    }
}
