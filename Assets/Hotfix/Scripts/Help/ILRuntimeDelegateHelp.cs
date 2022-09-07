using System;

namespace Tool
{
    public class ILRuntimeDelegateHelp
    {
        public static void RegisterDelegate(ILRuntime.Runtime.Enviorment.AppDomain appDomain)
        {
            //Button ����¼���ί��ע��
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<Predicate<object>>((act) =>
            {
                return new Predicate<object>((obj) =>
                {
                    return ((Func<object, bool>)act)(obj);
                });
            });

            appDomain.DelegateManager.RegisterMethodDelegate<int, object, Action>();
            appDomain.DelegateManager.RegisterMethodDelegate<int, object>();
            appDomain.DelegateManager.RegisterFunctionDelegate<object, bool>();
        }
    }
}
