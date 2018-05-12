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
/// 连接状态
/// </summary>
public enum Status
{
    None,
    Connected,
    Disconnected
}

/// <summary>
/// 处理网络连接
/// </summary>
public class Connector
{
    //连接socket
    private Socket connSocket;
    //buff大小
    private const int BUFFER_SIZE = 1024;
    private byte[] readBuff = new byte[BUFFER_SIZE];
    //buff count
    private int readBuffCount = 0;
    //粘包分包
    private Int32 msgLength = 0;
    private byte[] lenBytes = new byte[sizeof(Int32)];

    //send 相关参数
    private byte[] sendBuff = new byte[BUFFER_SIZE];
    private int sendBuffCount = 0;
    private Int32 sendMsgLength = 0;
    private byte[] sendLenBytes = new byte[sizeof(Int32)];

    /// <summary>
    /// 协议
    /// </summary>
    public BaseProtocol protocol;

    //心跳时间
    public float lastTickTime = 0;
    public float heartBeatTime=2;

    /// <summary>
    /// 消息分发
    /// </summary>
    public MsgDistribution msgDistribution = new MsgDistribution();

    /// <summary>
    /// 连接状态
    /// </summary>
    public Status status = Status.None;

    public Connector()
    {
        //协议
        protocol = new BytesProtocol();
    }

    /// <summary>
    /// 更新函数
    /// </summary>
    public void Update()
    {
        //消息
        msgDistribution.Update();
        //心跳
        //定时发送一个心跳包
        if(status==Status.Connected)
        {
            if(Time.time>heartBeatTime+lastTickTime)
            {
                BaseProtocol proto = NetMgr.GetHeartBeatProtocol();
                Send(proto);
                lastTickTime = Time.time;
            }
        }

    }

    /// <summary>
    /// socket连接
    /// </summary>
    public bool Connect()
    {
        try
        {
            connSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1995);
            connSocket.Connect(iPEndPoint);

            connSocket.BeginReceive(
                readBuff,
                readBuffCount,
                BUFFER_SIZE-readBuffCount,
                SocketFlags.None,
                ReceiveCb,
                readBuff
                );

            status = Status.Connected;
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"连接失败："+e.Message);
            return false;
        }
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <returns></returns>
    public bool Close()
    {
        try
        {
            connSocket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"关闭失败：{e.Message}");
            return false;
        }
    }

    /// <summary>
    /// 发送协议
    /// </summary>
    /// <param name="protocol"></param>
    public bool Send(BaseProtocol _protocol)
    {
        if(status!=Status.Connected)
        {
            Debug.Log("请先联网");
            return false;
        }

        //已经是编码后的协议
        sendBuff = _protocol.Encode();
        sendBuffCount =sendBuff.Length;

        connSocket.BeginSend(
            sendBuff,
            0,
            sendBuffCount,
            SocketFlags.None,
            SendCb,
            sendBuff
            );
        return true;
    }

    /// <summary>
    /// 发送协议并设置回调函数
    /// </summary>
    /// <param name="_protocol">协议名</param>
    /// <param name="nameCb">Once Action name</param>
    /// <param name="">Once Action</param>
    /// <returns></returns>
    public bool Send(BaseProtocol  _protocol,string nameCb,Action<BaseProtocol> action)
    {
        if(status!=Status.Connected)
        {
            return false;
        }
        msgDistribution.AddOnceListener(nameCb,action);
        return Send(_protocol);
    }

    /// <summary>
    /// 发送协议
    /// </summary>
    /// <param name="_protocol"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Send(BaseProtocol _protocol,Action<BaseProtocol> action)
    {
        string cbName = _protocol.GetName();
        if(string.IsNullOrEmpty(cbName))
        {
            return false;
        }
        return Send(_protocol,cbName, action);
    }

    /// <summary>
    /// 异步发送回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCb(IAsyncResult ar)
    {
        try
        {
            int count = connSocket.EndSend(ar);
            if (count <= 0)
            {
                return;
            }
            if(count==sendBuffCount)
            {
                return;
            }
            int remain =sendBuffCount - count;
            //清除已经发送的数据
            Array.Copy(
                sendBuff,
                count,
                sendBuff,
                0,
                remain
                );

            sendBuffCount = remain;

            if (sendBuffCount > 0)
            {
                //递归发送
                connSocket.BeginSend(
                    sendBuff,
                    0,
                    sendBuffCount,
                    SocketFlags.None,
                    SendCb,
                    sendBuff
                    );
            }
        }
        catch (Exception e)
        {
            Debug.Log($"SendCb异常：{e.Message}");
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
            int count = connSocket.EndReceive(ar);
            if (count <= 0)
            {
                return;
            }
            else
            {
                readBuffCount += count;
                ReceiveProcessData();
                //继续接收
                connSocket.BeginReceive(
                    readBuff,
                    readBuffCount,
                    BUFFER_SIZE-readBuffCount,
                    SocketFlags.None,
                    ReceiveCb,
                    readBuff);
            }
        }
        catch (Exception e)
        {
            //TODO:异常关闭情况
            Debug.Log($"ReceiveCb异常:{e.Message}");
            status = Status.None;
        }
    }

    /// <summary>
    /// 处理buff数据 分包
    /// </summary>
    /// <param name="conn">conn 连接</param>
    private void ReceiveProcessData()
    {
        ///小于长度字节 不是一个长度的字节长度
        if (readBuffCount < sizeof(Int32))
        {
            return;
        }
        try
        {
            //获取消息的长度
            Array.Copy(readBuff,lenBytes,sizeof(Int32));
            msgLength = BitConverter.ToInt32(lenBytes, 0);

            //判断是否够一条消息的长度
            if (readBuffCount < msgLength)
            {
                return;
            }
            ////使用协议解析 重点理解这里
            ///    消息体形式：(bytes消息长度|长度 协议|长度 参数1|长度 参数2)
            //TODO:
            BaseProtocol proto = protocol.Decode(readBuff, sizeof(Int32),msgLength - sizeof(Int32));
            lock (msgDistribution)
            {
                msgDistribution.msgList.Add(proto);
            }

            //清除已经处理的消息
            int length = readBuffCount - msgLength;
            Array.Copy(
                readBuff,
                msgLength,
                readBuff,
                0,
                length);
            readBuffCount = length;

            if (readBuffCount > 0)
            {
                ReceiveProcessData();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}
