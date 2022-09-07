using System.Collections.Generic;
using Hotfix.Manager;
using UnityEngine;
using Tool;
using System.Linq;
using System;
using System.Reflection;
using Hotfix.UI;

namespace Hotfix
{
    public class HotfixLaunch
    {
        static List<IManager> managerList;
        static List<Type> allTypes;

        public static void Start()
        {
            Debug.Log("这里是热更的入口 HotfixLaunch");
            //RegisterAllMgr();

            //获取Hotfix.dll内部定义的类
            allTypes = new List<Type>();
            managerList = new List<IManager>();

            var values = ILRuntimeHelp.appDomain.LoadedTypes.Values.ToList();
            foreach (var v in values)
            {
                allTypes.Add(v.ReflectionType);
            }
            #region 以下方法无效
            //1
            //var assembly = Assembly.GetAssembly(typeof(HotfixLaunch));
            //if (assembly == null)
            //{
            //    Debug.LogError("当前dll is null");
            //    return;
            //}
            //allTypes = assembly.GetTypes().ToList();
            //2
            //var types = Assembly.GetExecutingAssembly().GetTypes();
            //foreach(var t in types)
            //{
            //    allTypes.Add(t);
            //}
            #endregion
            allTypes = allTypes.Distinct().ToList();

            //获取hotfix的管理类，并启动
            foreach (var t in allTypes)
            {
                try
                {
                    if (t != null && t.BaseType != null && t.BaseType.FullName != null)
                    {
                        if (t.BaseType.FullName.Contains(".ManagerBase`"))
                        {
                            Debug.Log("ManagerBase 加载管理器-" + t);
                            var manager = t.BaseType.GetProperty("Instance").GetValue(null, null) as IManager;
                            managerList.Add(manager);
                            continue;
                        }
                        //else if (t.BaseType.FullName.Contains(".ManagerBaseWithAttr`"))
                        //{
                        //    Debug.Log("ManagerBaseWithAttr 加载管理器-" + t);
                        //    var manager = t.BaseType.BaseType.GetProperty("Instance").GetValue(null, null) as IManager;
                        //    managerList.Add(manager);
                        //    continue;
                        //}
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            Debug.Log("managerList.Count=" + managerList.Count);
            foreach (var manager in managerList)
            {
                manager.Init();
            }

            Launch.OnUpdate = Update;
            Launch.OnLateUpdate = LateUpdate;
            Launch.OnFixedUpdate = FixedUpdate;
            Launch.OnApplicationQuitAction = ApplicationQuit;

            foreach (var manager in managerList)
            {
                manager.Start();
            }

            //UI 初始化
            foreach (var t in allTypes)
            {
                UIManager.Instance.CheckType(t);
            }
            UIEventManager.ShowView((int)ViewId.UseMicrophone);
        }

        static void RegisterAllMgr()
        {
            managerList = new List<IManager>();
            managerList.Add(DigitalManager.Instance);
        }

        static void Update()
        {
            foreach (var manager in managerList)
            {
                manager.Update();
            }
        }

        static void LateUpdate()
        {
            foreach (var manager in managerList)
            {
                manager.LateUpdate();
            }
        }

        static void FixedUpdate()
        {
            foreach (var manager in managerList)
            {
                manager.FixedUpdate();
            }
        }

        static void ApplicationQuit()
        {
            foreach (var manager in managerList)
            {
                manager.OnApplicationQuit();
            }
        }
    }
}
