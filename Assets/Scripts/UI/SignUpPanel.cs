using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SignUp面板对应的类 包含此面板的操作方法和生命周期
/// </summary>
public class SignUpPanel : BasePanel
{
    /// <summary>
    /// signup Btn
    /// </summary>
    private Button signUpBtn;
    /// <summary>
    /// Already Register Btn
    /// </summary>
    private Button alreadyRegisBtn;
    /// <summary>
    /// username inputfield
    /// </summary>
    private InputField userNameIF;
    /// <summary>
    /// password inputfield
    /// </summary>
    private InputField pwIF;
    /// <summary>
    /// eMail InputField
    /// </summary>
    private InputField emailIF;

    #region 生命周期函数

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "SignUp";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        ///双重验证 ui控件未初始化 这里只初始化一次 如果频繁Find会使GC暴涨 主要是之前的
        ///赋值操作的变量会被GC回收
        if (signUpBtn == null && alreadyRegisBtn == null)
        {
            //get UI Controller
            Transform objTrans = panelObj.transform;
            signUpBtn = objTrans.Find("SignupBtn").GetComponent<Button>();
            alreadyRegisBtn = objTrans.Find("AlreadyRegisBtn").GetComponent<Button>();
            userNameIF = objTrans.Find("FieldUsername/InputField").GetComponent<InputField>();
            emailIF = objTrans.Find("FieldEmail/InputField").GetComponent<InputField>();
            pwIF = objTrans.Find("FieldPassword/InputField").GetComponent<InputField>();

            //add click event
            signUpBtn.onClick.AddListener(OnSignUpBtn);
            alreadyRegisBtn.onClick.AddListener(OnAlreadyRegisBtn);
        }
    }

    #endregion

    #region 自定义的UI event处理事件

    private void OnAlreadyRegisBtn()
    {
        //TODO:
        PanelMgr.instance.OpenPanel<LoginPanel>("");
    }

    /// <summary>
    /// 注册按钮事件
    /// </summary>
    private async void OnSignUpBtn()
    {
        ////TODO:
        //Debug.Log("click OnSignUpBtn");

        ////test
        //PanelMgr.instance.OpenTip<ErrorTip>("", "注册失败");
        ////test

        //if (userNameIF.text==""||pwIF.text==""||emailIF.text=="")
        //{
        //    Debug.Log("请填完注册信息");
        //    return;
        //}
        //if(NetMgr.connector.status!=Status.Connected)
        //{
        //    NetMgr.connector.Connect();
        //}
        //BytesProtocol proto = new BytesProtocol();
        //proto.AddString("Register");
        //proto.AddString(userNameIF.text);
        //proto.AddString(pwIF.text);
        //proto.AddString(emailIF.text);
        //Debug.Log($"发送：{proto.GetName()}协议");
        //if(!NetMgr.connector.Send(proto,OnRegisterEvent))
        //{
        //    //TODO:
        //    Debug.Log("注册失败");
        //}

        if (userNameIF.text == "" || pwIF.text == "" || emailIF.text == "")
        {
            PanelMgr.instance.OpenTip<ErrorTip>("", "请填完注册信息");
            return;
        }

        AVUser user = new AVUser();
        user.Username = userNameIF.text;
        user.Email = emailIF.text;
        user.Password = pwIF.text;
        try
        {
            await user.SignUpAsync();
            OnRegisterSuccess();
        }
        catch (Exception)
        {
            OnRegisterFail();
        }
    }

    /// <summary>
    /// 注册失败
    /// </summary>
    private void OnRegisterFail()
    {
        PanelMgr.instance.OpenTip<ErrorTip>("","注册失败");
    }

    /// <summary>
    /// 注册成功
    /// </summary>
    private void OnRegisterSuccess()
    {
        PanelMgr.instance.OpenPanel<UserPanel>("");
    }

    /// <summary>
    /// 注册协议的回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnRegisterEvent(BaseProtocol obj)
    {
        BytesProtocol proto = obj as BytesProtocol;
        if(proto!=null)
        {
            int start = 0;
            string protoName = proto.GetString(start,ref start);
            int status = proto.GetInt(start,ref start).Value;
            if(status==1)
            {
                Debug.Log("注册成功");
                //TODO:打开别的面板
            }
            else
            {
                Debug.Log("注册失败");
            }
        }
    }

    #endregion
}
