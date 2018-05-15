using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Login面板类
/// </summary>
public class LoginPanel :BasePanel
{
    /// <summary>
    /// 登录按钮
    /// </summary>
    private Button loginBtn;
    /// <summary>
    /// 忘记密码按钮
    /// </summary>
    private Button forgotPWBtn;
    /// <summary>
    /// 创建新的账号按钮
    /// </summary>
    private Button createNewBtn;
    /// <summary>
    /// 用户名输入框
    /// </summary>
    private InputField userNameIF;
    /// <summary>
    /// 密码输入框
    /// </summary>
    private InputField pwIF;

    #region 生命周期函数

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "Login";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        ///双重验证 ui控件未初始化 这里只初始化一次 如果频繁Find会使GC暴涨 主要是之前的
        ///赋值操作的变量会被GC回收

        if (loginBtn == null && forgotPWBtn == null)
        {
            Transform objTrans = this.panelObj.transform;
            loginBtn = objTrans.Find("LoginBtn").GetComponent<Button>();
            forgotPWBtn = objTrans.Find("ForgotPWBtn").GetComponent<Button>();
            createNewBtn = objTrans.Find("CreateNewBtn").GetComponent<Button>();
            userNameIF = objTrans.Find("FieldUsername/InputField").GetComponent<InputField>();
            pwIF = objTrans.Find("FieldPassword/InputField").GetComponent<InputField>();

            loginBtn.onClick.AddListener(OnLoginBtn);
            forgotPWBtn.onClick.AddListener(OnForgotPWBtn);
            createNewBtn.onClick.AddListener(OnCreateNewBtn);
        }
    }

    public override void OnShowed()
    {
        base.OnShowed();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnClosing()
    {
        base.OnClosing();
    }

    public override void OnClosed()
    {
        base.OnClosed();
    }

    #endregion

    #region 定义事件函数

    /// <summary>
    /// 点击了CreateNew按钮，切换到signup面板 关闭登录按钮
    /// </summary>
    private void OnCreateNewBtn()
    {
        PanelMgr.instance.OpenPanel<SignUpPanel>("");
    }

    private void OnForgotPWBtn()
    {
        //TODO:点击忘记密码按钮，找回密码
    }

    /// <summary>
    /// 登录按钮
    /// </summary>
    private void OnLoginBtn()
    {
        //test
        PanelMgr.instance.OpenPanel<UserPanel>("");
        //test

        //TODO:验证登录信息
        //客户端检验
        if(userNameIF.text==""||pwIF.text=="")
        {
            //TODO:
            Debug.Log("用户名密码不能为空");
            return;
        }

        if(NetMgr.connector.status!=Status.Connected)
        {
            if(!NetMgr.connector.Connect())
            {
                //TODO:
                Debug.Log("网络连接失败");
                return;
            }
        }

        //发送Login协议
        BytesProtocol proto = new BytesProtocol();
        proto.AddString("Login");
        proto.AddString(userNameIF.text);
        proto.AddString(pwIF.text);

        NetMgr.connector.Send(proto, OnLoginBack);

    }

    /// <summary>
    /// 登录协议的回调函数 根据返回的协议判断登录是否成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoginBack(BaseProtocol obj)
    {
        BytesProtocol proto = obj as BytesProtocol;
        int start=0;
        string protoName = proto.GetString(start, ref start);
        int status = proto.GetInt(start,ref start).Value;
        if(status==1)
        {
            Debug.Log("登录成功");
            NetMgr.connector.status = Status.Connected;
            //TODO:切换到用户信息界面
        }
        else
        {
            Debug.Log("登录失败");
            NetMgr.connector.status = Status.None;
            NetMgr.connector.Close();
            //TODO:反馈信息 重新登录
        }
    }

    #endregion

}
