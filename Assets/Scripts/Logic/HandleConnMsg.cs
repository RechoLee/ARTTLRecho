using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HandleConnMsg
/// </summary>
public class HandleConnMsg
{
    
    /// <summary>
    /// 处理心跳消息
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="_protocol"></param>
    public void MsgHeartBeat(Conn conn,BaseProtocol _protocol)
    {

    }

    /// <summary>
    /// 接受到服务器发过来的Login协议  判断能否登录
    /// 服务器：Login|1/0
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="_protocol"></param>
    public void MsgLogin(Conn conn,BaseProtocol _protocol)
    {
        BytesProtocol proto = _protocol as BytesProtocol;
        int start = 0;
        string protoName = proto.GetString(start,ref start);
        int status = proto.GetInt(start,ref start).Value;
        if(status==0)
        {
            //TODO:登录不成功
        }
        else if(status==1)
        {
            //TODO:登录成功
        }
    }


    /// <summary>
    /// 注册协议 判断注册是否成功
    /// 服务器： Register|1/0
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="_protocol"></param>
    public void MsgRegister(Conn conn,BaseProtocol _protocol)
    {
        BytesProtocol proto = _protocol as BytesProtocol;

        int start = 0;
        string protoName = proto.GetString(start,ref start);
        int status = proto.GetInt(start,ref start).Value;

        if(status==0)
        {
            //TODO:注册不成功
        }
        else if(status==1)
        {
            //TODO：注册成功
        }

    }
}
