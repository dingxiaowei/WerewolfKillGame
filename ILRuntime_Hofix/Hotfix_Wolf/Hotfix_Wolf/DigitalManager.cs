using UnityEngine;

namespace Hotfix.Manager
{
    //测试
    class DigitalManager : ManagerBase<DigitalManager>
    {
        public override void Init()
        {
            base.Init();
            Debug.Log("DigitalManager 初始化");
        }
        public override void Start()
        {
            base.Start();
            Debug.Log("DigitalManager Start");
        }
        public override void Update()
        {
            base.Update();          
        }
    }
}
