using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

namespace Tool
{
    public class ILRuntimeHelp
    {
        public static ILRuntime.Runtime.Enviorment.AppDomain appDomain;
        public  static int TT;
        static MemoryStream m_hotfixDllMemoryStream;
        static MemoryStream m_hotfixPdbMemoryStream;

        public static IEnumerator LoadILRuntime(Action loadedFinish)
        {
            appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();

#if UNITY_ANDROID
            var uri = new System.Uri("http://116.205.247.142:8080/dxw/Wolf/Hotfix_Wolf.dll");
            var webRequest = UnityWebRequest.Get(uri.AbsoluteUri);
            //var webRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/Hotfix_Wolf.dll");
#else
            var webRequest = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/Hotfix_Wolf.dll");
#endif
            yield return webRequest.SendWebRequest();

            byte[] dll = null;
            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                dll = webRequest.downloadHandler.data;
            }
            else
            {
                Debug.LogError("Download Error:" + webRequest.error);
            }
            webRequest.Dispose();

#if UNITY_ANDROID
            uri = new System.Uri("http://116.205.247.142:8080/dxw/Wolf/Hotfix_Wolf.pdb");
            webRequest = UnityWebRequest.Get(uri.AbsoluteUri);
            //webRequest = UnityWebRequest.Get(Application.streamingAssetsPath + "/Hotfix_Wolf.pdb");
#else
            webRequest = UnityWebRequest.Get("file:///" + Application.streamingAssetsPath + "/Hotfix_Wolf.pdb");
#endif
            yield return webRequest.SendWebRequest();

            byte[] pdb = null;
            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                pdb = webRequest.downloadHandler.data;
            }
            else
            {
                Debug.LogError("Download Error:" + webRequest.error);
            }

            m_hotfixDllMemoryStream = new MemoryStream(dll);
            m_hotfixPdbMemoryStream = new MemoryStream(pdb);

            appDomain.LoadAssembly(m_hotfixDllMemoryStream, m_hotfixPdbMemoryStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

            webRequest.Dispose();
            webRequest = null;

            ILRuntimeDelegateHelp.RegisterDelegate(appDomain);
            ILRuntimeAdapterHelp.RegisterCtossBindingAdaptor(appDomain);

            if (Application.isEditor)
            {
                appDomain.DebugService.StartDebugService(56000);
            }

            loadedFinish?.Invoke();
        }

        public static void Dispose()
        {
            m_hotfixDllMemoryStream?.Dispose();
            m_hotfixPdbMemoryStream?.Dispose();
        }
    }
}