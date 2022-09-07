﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class Connect : MonoBehaviour {

    private float time;

    public Dropdown[] dropDown;

    public GameObject ShowPanel;
    public Text Msg;

    public Toggle[] toggles;

    public InputField[] roomids;

    public Button[] creates;
    public Button[] joins;

    void OnApplicationQuit()
    {
        try
        {
            if (Server.Connected())
            {
                Server.Close();
            }
        }
        catch
        {
        }
        finally
        {
            Application.Quit();
        }
    }

    public void JoinGame(int index)
    {
        joins[index].enabled = false;
        int roomid;
        string roomidstr = roomids[index].text;
        bool f = int.TryParse(roomidstr, out roomid);
        if (string.IsNullOrEmpty(roomidstr) || !f)
        {
            ShowMsg("请输入正确的房间号！");
        }
        else
        {
            try
            {

                Server.socket.SendMsg(Command.IsJoinGame(roomid, index));
                string IsJoin = "";
                while (true)
                {
                    string msg = Server.socket.GetMsg();
                    if (msg != "wait" && !string.IsNullOrEmpty(msg) && ( msg=="False" || msg=="True" || msg=="Playing" ))
                    {
                        IsJoin = msg;
                        break;
                    }
                }
                if (IsJoin == "False")
                {
                    ShowMsg("房间不存在！");
                }
                else if (IsJoin == "Playing")
                {
                    ShowMsg("房间正在游戏中！");
                }
                else if (IsJoin == "True")
                {
                    switch (index)
                    {
                        case 0:
                            string msg = Command.JoinGame(Server.username, roomid, Server.lastname);
                            StartCoroutine(SendMsg(msg));
                            Server.roomid = roomid.ToString();
                            Server.IsFangZhu = false;
                            SceneManager.LoadScene(2);
                            break;
                        case 1:
                            string msg1 = Command.JoinGame(Server.username, roomid, Server.lastname);
                            StartCoroutine(SendMsg(msg1));
                            Server.roomid = roomid.ToString();
                            Server.IsFangZhu = false;
                            SceneManager.LoadScene(3);
                            break;
                        case 2:
                            ShowMsg("还未开发，敬请期待");
                            break;
                        case 3:
                            ShowMsg("还未开发，敬请期待");
                            break;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        joins[index].enabled = true;
    }

    IEnumerator SendMsg(string msg)
    {
        Server.socket.SendMsg(msg);
        yield return new WaitForSeconds(0);
    }

    public void CreateGame(int index)
    {
        creates[index].enabled = false;
        try
        {
            switch (index)
            {
                case 0:
                    int num = -5;
                    List<Toggle> npcs = new List<Toggle>();
                    foreach (Toggle toggle in toggles)
                    {
                        if (toggle.isOn)
                        {
                            npcs.Add(toggle);
                        }
                    }
                    bool flag = int.TryParse(dropDown[index].value.ToString(), out num);
                    num += 4;
                    if (!flag)
                    {
                        ShowMsg("人数转换错误！");
                    }
                    else if (!toggles[9].isOn && !toggles[10].isOn && !toggles[11].isOn)
                    {
                        ShowMsg("必须至少选择1个平民！");
                    }
                    else if (!toggles[0].isOn && !toggles[1].isOn && !toggles[7].isOn && !toggles[8].isOn)
                    {
                        ShowMsg("必须至少选择1个狼人！");
                    }
                    else if (npcs.Count != num + 3)
                    {
                        ShowMsg("角色与玩家不匹配！");
                    }
                    else if (npcs.Count == num + 3)
                    {
                        List<string> shenfen = new List<string>();
                        for (int i = 0; i < toggles.Length;i++ )
                        {
                            if(toggles[i].isOn &&( i==0 || i==1))
                            {
                                shenfen.Add("狼人"+(i+1));
                            }
                            else if (toggles[i].isOn && i == 2)
                            {
                                shenfen.Add("预言家");
                            }
                            else if (toggles[i].isOn && i == 3)
                            {
                                shenfen.Add("盗贼");
                            }
                            else if (toggles[i].isOn && i == 4)
                            {
                                shenfen.Add("小女孩");
                            }
                            else if (toggles[i].isOn && i == 5)
                            {
                                shenfen.Add("守夜人");
                            }
                            else if (toggles[i].isOn && i == 6)
                            {
                                shenfen.Add("酒鬼");
                            }
                            else if (toggles[i].isOn && i == 7)
                            {
                                shenfen.Add("狼王");
                            }
                            else if (toggles[i].isOn && i == 8)
                            {
                                shenfen.Add("爪牙");
                            }
                            else if (toggles[i].isOn && (i == 9 || i == 10 || i == 11))
                            {
                                shenfen.Add("平民"+(i-8));
                            }
                        }
                        StartCoroutine(CreateGameIE(Server.username, num, index, Server.lastname,shenfen));
                    }
                    else
                    {
                        ShowMsg("创建房间错误，联系管理员！");
                    }

                    break;
                case 1:
                    int num1 = -5;
                    bool flag1 = int.TryParse(dropDown[index].value.ToString(), out num1);
                    if (!flag1)
                    {
                        ShowMsg("人数转换错误！");
                    }
                    else
                    {
                        num1 += 4;
                        StartCoroutine(CreateGameIE(Server.username, num1, index, Server.lastname, null));
                    }
                    break;
                case 2:
                    ShowMsg("还未开发，敬请期待");
                    break;
                case 3:
                    ShowMsg("还未开发，敬请期待");
                    break;
            }
        }
        catch
        {
            return;
        }
        creates[index].enabled = true;
    }

    IEnumerator CreateGameIE(string username,int num,int index,string lastname,List<string> shenfen)
    {
        try 
        {
            string sendmsg = Command.CreateRoom(username, num, index, 0, lastname, shenfen);
            Server.socket.SendMsg(sendmsg);
            bool f = false;
            while (true)
            {
                string msg = Server.socket.GetMsg();
                int roomidstr;
                if (msg != "wait" && !string.IsNullOrEmpty(msg) && int.TryParse(msg,out roomidstr))
                {
                    Server.roomid = roomidstr.ToString();
                    Server.playernum = num;
                    Server.IsFangZhu = true;
                    f = true;
                    break;
                }
            }
            if (f)
            {
                switch (index)
                {
                    case 0:
                        SceneManager.LoadScene(2);
                        break;
                    case 1:
                        SceneManager.LoadScene(3);
                        break;
                    case 2:
                        SceneManager.LoadScene(4);
                        break;
                    case 3:
                        SceneManager.LoadScene(5);
                        break;
                }
                
            }        
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        yield return new WaitForSeconds(0);
    }

    private void ShowMsg(string msg)
    {
        Msg.text = msg;
        ShowPanel.SetActive(true);
        StartCoroutine(Toggle());
    }

    private IEnumerator Toggle()
    {
        yield return new WaitForSeconds(1.0f);
        ShowPanel.SetActive(false);
    }

}
