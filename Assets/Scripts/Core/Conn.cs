using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// 客户端socket连接对象
/// </summary>
public class Conn
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
    public User user;

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

        ////TODO:心跳处理
        //lastTickTime = Sys.GetTimeStamp();
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
            Console.WriteLine(e.Message);
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
        if (user != null)
        {
            //TODO:用户下线操作

            return;
        }

        Console.WriteLine($"用户断开连接，地址为：{GetAddress()}");

        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        isUse = false;
    }

    ///// <summary>
    ///// 发送协议
    ///// </summary>
    ///// <param name="protocol"></param>
    //public void Send(BaseProtocol protocol)
    //{
    //    //TODO:
    //    ServNet.instance.Send(this, protocol);
    //}
}
