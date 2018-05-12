using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;

/// <summary>
/// 处理网络连接
/// </summary>
public class Connector
{

    public Socket connSocket;

    public Conn _conn;

    public string testText="";

    /// <summary>
    /// 协议
    /// </summary>
    BaseProtocol protocol;

    /// <summary>
    /// 单例
    /// </summary>
    public static Connector instance=null;

    /// <summary>
    /// Conn消息处理类
    /// </summary>
    public HandleConnMsg handleConnMsg;
    /// <summary>
    /// User消息处理类
    /// </summary>
    public HandleUserMsg handleUserMsg;
    /// <summary>
    /// User event处理类
    /// </summary>
    public HandleUserEvent handleUserEvent;

    public Connector()
    {
        if (instance == null)
            instance = this;

        //协议
        protocol = new BytesProtocol();

        //初始化消息事件处理类
        handleUserEvent = new HandleUserEvent();
        handleUserMsg = new HandleUserMsg();
        handleConnMsg = new HandleConnMsg();
    }


    /// <summary>
    /// socket连接
    /// </summary>
    public void Connect()
    {
        connSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _conn = new Conn();
        _conn.Init(connSocket);

        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),1995);
        connSocket.Connect(iPEndPoint);

        connSocket.BeginReceive(
            _conn.readBuff,
            _conn.buffCount,
            _conn.BuffRemain(),
            SocketFlags.None,
            ReceiveCb,
            _conn
            );
    }

    /// <summary>
    /// 发送协议
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="protocol"></param>
    public void Send(Conn conn, BaseProtocol _protocol)
    {
        //已经是编码后的协议
        conn.sendBuff = _protocol.Encode();
        conn.sendBuffCount = conn.sendBuff.Length;

        conn.socket.BeginSend(
            conn.sendBuff,
            0,
            conn.sendBuffCount,
            SocketFlags.None,
            SendCb,
            conn
            );
    }

    /// <summary>
    /// 异步发送回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCb(IAsyncResult ar)
    {
        try
        {
            Conn conn = ar.AsyncState as Conn;

            if (conn != null)
            {
                int count = conn.socket.EndSend(ar);
                if (count <= 0)
                {
                    return;
                }
                int remain = conn.sendBuffCount - count;
                //清除已经发送的数据
                Array.Copy(
                    conn.sendBuff,
                    count,
                    conn.sendBuff,
                    0,
                    remain
                    );

                conn.sendBuffCount = remain;

                if (conn.sendBuffCount > 0)
                {
                    //递归发送
                    conn.socket.BeginSend(
                        conn.readBuff,
                        0,
                        conn.sendBuffCount,
                        SocketFlags.None,
                        SendCb,
                        conn
                        );
                }

            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SendCb异常：{e.Message}");
        }
    }

    /// <summary>
    /// 异步接收回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            Conn conn = ar.AsyncState as Conn;
            if (conn != null)
            {
                
                int count = conn.socket.EndReceive(ar);

                if (count <= 0)
                {
                    return;
                }
                else
                {
                    conn.buffCount += count;
                    //TODO:
                    ReceiveProcessData(conn);

                    //继续接收
                    conn.socket.BeginReceive(
                        conn.readBuff,
                        conn.buffCount,
                        conn.BuffRemain(),
                        SocketFlags.None,
                        ReceiveCb,
                        conn);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"socket 接收异常：{e.Message}");
        }
    }

    /// <summary>
    /// 处理buff数据 分包
    /// </summary>
    /// <param name="conn">conn 连接</param>
    private void ReceiveProcessData(Conn conn)
    {
        ///小于长度字节 不是一个长度的字节长度
        if (conn.buffCount < sizeof(Int32))
        {
            return;
        }

        try
        {
            //获取消息的长度
            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);

            //判断是否够一条消息的长度
            if (conn.buffCount < conn.msgLength)
            {
                return;
            }

            ////使用协议解析 重点理解这里
            ///    消息体形式：(bytes消息长度|长度 协议|长度 参数1|长度 参数2)
            //TODO:
            BaseProtocol proto = protocol.Decode(conn.readBuff, sizeof(Int32), conn.msgLength - sizeof(Int32));
            HandleMsg(conn, proto);

            //清除已经处理的消息
            int length = conn.buffCount - conn.msgLength;
            Array.Copy(
                conn.readBuff,
                conn.msgLength,
                conn.readBuff,
                0,
                length);
            conn.buffCount = length;

            if (conn.buffCount > 0)
            {
                ReceiveProcessData(conn);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"异常 {e.Message}");
        }
    }

    /// <summary>
    /// 处理消息 反射实现
    /// </summary>
    public void HandleMsg(Conn conn,BaseProtocol _protocol)
    {
        //int start=0;
        //BytesProtocol proto = _protocol as BytesProtocol;
        //string protoName = proto.GetString(start,ref start);
        //int protoArgs = proto.GetInt(start,ref start).Value;

        //this.testText = protoName + protoArgs.ToString();

        string name = _protocol.GetName();
        string methodName = "Msg" + name;

        //连接协议分发
        if((conn.user==null)||(name=="HeartBeat")||(name=="Logout"))
        {
            MethodInfo mi=handleConnMsg.GetType().GetMethod(methodName);

            if (mi == null)
            {
                //没有处理方法
                return;
            }

            object[] objs = new object[] {conn,_protocol };
            mi.Invoke(handleConnMsg,objs);
        }
        ///交给user handle处理
        else
        {
            //
            MethodInfo mi = handleUserMsg.GetType().GetMethod(methodName);
            if(mi==null)
            {
                return;
            }

            object[] objs = new object[] {conn,_protocol };
            mi.Invoke(handleUserMsg,objs);
        }

    }
}
