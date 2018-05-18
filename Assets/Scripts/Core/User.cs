using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    //用户数据
    public UserData userData;

    /// <summary>
    /// 用户连接对象
    /// </summary>

    public User(UserData data)
    {
        this.userData = data;
    }

    /// <summary>
    /// 供逻辑层调用的send
    /// </summary>
    /// <param name="proto"></param>
    public void Send(BaseProtocol proto)
    {

    }
}
