using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Linq;

public class Connector : MonoBehaviour
{

    public Socket connSocket;

    // Use this for initialization
    void Start ()
    {
        Connect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// socket连接
    /// </summary>
    public void Connect()
    {
        connSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Conn conn = new Conn();
        conn.Init(connSocket);

        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),1995);
        connSocket.Connect(iPEndPoint);

        string str = "HelloWorld";
        byte[] msgBytes = Encoding.UTF8.GetBytes(str);
        byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);
        byte[] sendBuff = lengthBytes.Concat(msgBytes).ToArray();

        Send(conn, str);

        connSocket.BeginReceive(
            conn.readBuff,
            conn.buffCount,
            conn.BuffRemain(),
            SocketFlags.None,
            ReceiveCb,
            conn
            );
    }

    /// <summary>
    /// 封装了给客户端发送消息的方法，可以确保消息全部被发出
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="str"></param>
    private void Send(Conn conn, string str)
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
            Debug.LogError($"Send方法中异常：{e.Message}");
        }
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


/// <summary>
/// Conn对象
/// </summary>
public class  Conn
{
    /// <summary>
    /// Buffer_Size 常量 
    /// </summary>
    public const int BUFFER_SIZE = 1024;
    /// <summary>
    /// 客户端连接socket
    /// </summary>
    public Socket socket;
    /// <summary>
    /// 此连接是否可用
    /// </summary>
    public bool isUse = false;
    /// <summary>
    /// 缓存客户端发送的数据buff池
    /// </summary>
    public byte[] readBuff = new byte[BUFFER_SIZE];

    //send相关的数据

    /// <summary>
    /// send的总的buff大小
    /// </summary>
    public byte[] sendBuff = new byte[BUFFER_SIZE];
    /// <summary>
    /// send的消息体的bytes
    /// </summary>
    public byte[] sendMsgBytes;
    /// <summary>
    /// send的buff 长度
    /// </summary>
    public int sendBuffCount = 0;
    /// <summary>
    /// send的消息体的长度的bytes
    /// </summary>
    public byte[] sendLengthBytes = new byte[sizeof(Int32)];
    /// <summary>
    /// send的消息体长度 int
    /// </summary>
    public Int32 sendMsgLength = 0;

    /// <summary>
    /// buff的大小
    /// </summary>
    public int buffCount = 0;
    /// <summary>
    /// 粘包分包
    /// </summary>
    public byte[] lenBytes = new byte[sizeof(Int32)];
    /// <summary>
    /// 消息长度
    /// </summary>
    public Int32 msgLength = 0;

    /// <summary>
    /// 心跳时间
    /// </summary>
    public long lastTickTime = long.MinValue;

    /// <summary>
    /// 对应的用户
    /// </summary>
    //public User user;

    /// <summary>
    /// 构造函数 初始化操作 变量初始化
    /// </summary>
    public Conn()
    {
        readBuff = new byte[BUFFER_SIZE];
        sendBuff = new byte[BUFFER_SIZE];
    }

    /// <summary>
    /// 初始化操作
    /// </summary>
    /// <param name="socket">传入套接字</param>
    public void Init(Socket _socket)
    {
        this.socket = _socket;
        isUse = true;
        buffCount = 0;

        //TODO:心跳处理
    }

    /// <summary>
    /// 获取剩余buff大小
    /// </summary>
    /// <returns>剩余buff长度</returns>
    public int BuffRemain()
    {
        return BUFFER_SIZE - buffCount;
    }

    /// <summary>
    /// 获取客户端的ip地址
    /// </summary>
    /// <returns>返回地址或者空，就是无法获取地址</returns>
    public string GetAddress()
    {
        if (!isUse)
            return "";
        try
        {
            return socket.RemoteEndPoint.ToString();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return "";
        }
    }


    /// <summary>
    /// 设置sendbuff 并设置相关参数
    /// </summary>
    /// <param name="msgBytes"></param>
    public void SetSendBuff(byte[] msgBytes)
    {
        sendMsgBytes = msgBytes;
        sendMsgLength = sendMsgBytes.Length;
        sendLengthBytes = BitConverter.GetBytes(sendMsgLength);
        sendBuff = sendLengthBytes.Concat(sendMsgBytes).ToArray();
        sendBuffCount = sendBuff.Length;
    }

    /// <summary>
    /// conn关闭操作 释放socket连接
    /// </summary>
    public void Close()
    {
        if (!isUse)
            return;
        //if (user != null)
        //{
        //    //TODO:用户下线操作

        //    return;
        //}

        Debug.Log($"断开连接，地址为：{GetAddress()}");

        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        isUse = false;
    }

    //public void Send(ProtocolBase protocol)
    //{
    //    //TODO:
    //}

}
