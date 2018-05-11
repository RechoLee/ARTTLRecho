using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 处理网络连接
/// </summary>
public class Connector
{

    public Socket connSocket;

    public Conn _conn;

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
    /// 封装了给客户端发送消息的方法，可以确保消息全部被发出
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="str"></param>
    public void Send(Conn conn, string str)
    {
        try
        {
            //调用setSendBuff
            conn.SetSendBuff(Encoding.UTF8.GetBytes(str));

            conn.socket.BeginSend(
                conn.sendBuff,
                0,
                conn.sendBuffCount,
                SocketFlags.None,
                SendCb,
                conn);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Send方法中异常：{e.Message}");
        }
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
                    Debug.Log("close");
                    //conn.Close();
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
            Debug.Log("消息过短");
            return;
        }

        try
        {
            //获取消息的长度
            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);

            //判断是否够一条消息的长度
            if (conn.buffCount < conn.msgLength + sizeof(Int32))
            {
                return;
            }

            //处理消息
            string str = Encoding.UTF8.GetString(conn.readBuff, sizeof(Int32), conn.msgLength);

            Debug.Log(str);

            //清除已经处理的消息
            int length = conn.buffCount - conn.msgLength - sizeof(Int32);
            Array.Copy(
                conn.readBuff,
                conn.msgLength + sizeof(Int32),
                conn.readBuff,
                0,
                length);

            conn.buffCount = length;
        }
        catch (Exception)
        {
            Debug.LogError("数组异常");
        }
        finally
        {
            if (conn.buffCount > 0)
            {
                ReceiveProcessData(conn);
            }
        }
    }
}
