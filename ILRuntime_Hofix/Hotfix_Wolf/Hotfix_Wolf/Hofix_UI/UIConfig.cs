using System.Collections.Generic;

/// <summary>
/// 界面定义
/// </summary>
public enum ViewId
{
    ChoiceGameView,
    UseMicrophone,
}

namespace Hotfix.UI
{
    /// <summary>
    /// 现在仅记录使用
    /// </summary>
    public class UIConfig
    {
        Dictionary<ViewId, string> dict = new Dictionary<ViewId, string>()
        {
            { ViewId.ChoiceGameView, "UI/ChoiceGameView" },
        };
    }
}
