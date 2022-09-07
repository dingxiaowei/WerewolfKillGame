using UnityEngine;
using Tool;
using System;

public class Launch : MonoBehaviour
{
    public static Action OnUpdate { get; set; }
    public static Action OnLateUpdate { get; set; }
    public static Action OnFixedUpdate { get; set; }
    public static Action OnApplicationQuitAction { get; set; }

    static ILRuntime.CLR.Method.IMethod ILDigitalFunc;

    #region ÉúÃüÖÜÆÚ
    private void Start()
    {
        StartCoroutine(ILRuntimeHelp.LoadILRuntime(OnILRuntimeInitialized));
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnApplicationQuitAction?.Invoke();
    }
    #endregion

    void OnILRuntimeInitialized()
    {
        ILRuntimeHelp.appDomain?.Invoke("Hotfix.HotfixLaunch", "Start", null, null);
        //var type = ILRuntimeHelp.appDomain.LoadedTypes["Hotfix.HotfixLaunch"];
        //ILDigitalFunc = type.GetMethod("onDigital", 2);
    }

    public static void ReistDigital(GameObject go, string json)
    {
        //ILRuntimeHelp.appDomain?.Invoke(ILDigitalFunc, null, go, json);
        //ILRuntimeHelp.appDomain?.Invoke("Hotfix_Wolf.HotfixLaunch", "onDigital", null, new object[] { go, json });
    }

    public static void UnRegistDigital(GameObject go)
    {
        
    }
}
