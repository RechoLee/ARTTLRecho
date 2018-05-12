using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 协议分发类
/// </summary>
public class MsgDistribution
{
    /// <summary>
    /// 每帧处理消息的数量
    /// </summary>
    public int num =2;

    /// <summary>
    /// 接收到的协议集合
    /// </summary>
    public List<BaseProtocol> msgList;
    /// <summary>
    /// 事件监听表
    /// </summary>
    public Dictionary<string, Action<BaseProtocol>> eventDict;

    /// <summary>
    /// 执行一次的事件监听表
    /// </summary>
    public Dictionary<string, Action<BaseProtocol>> onceDict;

    public MsgDistribution()
    {
        msgList = new List<BaseProtocol>();
        eventDict = new Dictionary<string, Action<BaseProtocol>>();
        onceDict = new Dictionary<string, Action<BaseProtocol>>();

    }

    /// <summary>
    /// 更新函数
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < num; i++)
        {
            if(msgList.Count>0)
            {
                DispatchMsgEvent(msgList[0]);
                lock (msgList)
                {
                    msgList.RemoveAt(0);
                }
            }
            else
            {
                break;
            }
        }

    }

    /// <summary>
    /// 消息分发
    /// </summary>
    /// <param name="_protocol"></param>
    public void DispatchMsgEvent(BaseProtocol _protocol)
    {
        string name = _protocol.GetName();
        if(string.IsNullOrEmpty(name))
        {
            return;
        }
        if(eventDict.ContainsKey(name))
        {
            eventDict[name](_protocol);
        }
        if(onceDict.ContainsKey(name))
        {
            onceDict[name](_protocol);
            onceDict[name] = null;
            lock (onceDict)
            {
                onceDict.Remove(name);
            }
        }
    }

    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddListener(string name,Action<BaseProtocol> action)
    {
        if(eventDict.ContainsKey(name))
        {
            eventDict[name] += action;
        }
        else
        {
            eventDict[name] = action;
        }
    }

    /// <summary>
    /// 添加一次消息的监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddOnceListener(string name,Action<BaseProtocol> action)
    {
        if(onceDict.ContainsKey(name))
        {
            onceDict[name] += action;
        }
        else
        {
            onceDict[name] = action;
        }
    }


    /// <summary>
    /// 移除监听的事件
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="action"></param>
    public void DeleteListener(string name,Action<BaseProtocol> action)
    {
        if(eventDict.ContainsKey(name))
        {
            eventDict[name] -= action;
            if (eventDict[name] == null)
                eventDict.Remove(name);
        }
    }

    /// <summary>
    /// 移除一次性的监听事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void DeleteOnceListener(string name,Action<BaseProtocol> action)
    {
        if(onceDict.ContainsKey(name))
        {
            onceDict[name] -= action;
            if (onceDict[name] == null)
                onceDict.Remove(name);
        }

    }
}