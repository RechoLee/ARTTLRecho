using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户的数据
/// </summary>
public class UserData
{
    private string name;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string pw;

    /// <summary>
    /// 密码
    /// </summary>
    public string PW
    {
        get { return pw; }
        set { pw = value; }
    }

    private string email;

    /// <summary>
    /// Email
    /// </summary>
    public string Email
    {
        get { return email; }
        set { email = value; }
    }

}

