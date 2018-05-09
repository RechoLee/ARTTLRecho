using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnLoginBtn()
    {
        //TODO:验证登录信息
    }

    #endregion

}
