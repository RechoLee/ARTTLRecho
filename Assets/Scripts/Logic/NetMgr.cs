using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理网络连接的类
/// </summary>
public class NetMgr
{
    /// <summary>
    /// 与平台相关的连接 可以接其他平台的连接 然后自己实现connector
    /// </summary>
    public static Connector connector = new Connector();

    /// <summary>
    /// 更新函数
    /// </summary>
    public static void Update()
    {
        connector.Update();
    }

    /// <summary>
    /// 返回一个心跳协议
    /// </summary>
    /// <returns></returns>
    public static BaseProtocol GetHeartBeatProtocol()
    {
        //心跳协议
        BytesProtocol proto = new BytesProtocol();
        proto.AddString("HeartBeat");
        return proto;
    }
}
