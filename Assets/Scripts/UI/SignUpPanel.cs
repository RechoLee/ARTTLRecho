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



    #endregion

    #region 自定义的UI event处理事件

    private void OnAlreadyRegisBtn()
    {
        //TODO:
        PanelMgr.instance.OpenPanel<LoginPanel>("");
    }

    private void OnSignUpBtn()
    {
        //TODO:
        Debug.Log("click OnSignUpBtn");
    }

    #endregion
}
