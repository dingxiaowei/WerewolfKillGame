using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Hotfix.UI
{
    [UI(ViewId.ChoiceGameView, "UI/ChoiceGameView/ChoiceGameView")]
    public class ChoiceGameView : UIBase
    {
        public ChoiceGameView(string resourcePath) : base(resourcePath)
        {

        }

        [TransformPath("btn_Exit")] private Button btn_Exit;

        public override void Init()
        {
            base.Init();
            btn_Exit.onClick.AddListener(funExit);
        }

        void funExit()
        {
            UIManager.Instance.HideView(ViewId.ChoiceGameView);
        }
    }
}
