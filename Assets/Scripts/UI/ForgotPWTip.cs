using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 忘记密码找回
/// </summary>
public class ForgotPWTip:BasePanel
{

    private InputField emailInput;
    private Button closeBtn;
    private Button okBtn;
    private Text errorText;

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "ForgotPWTip";
        this.layer = PanelLayer.Tip;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(closeBtn==null&&okBtn==null)
        {
            Transform trans = this.panelObj.transform;
            closeBtn = trans.Find("Content/CloseBtn").GetComponent<Button>();
            okBtn = trans.Find("Content/OkBtn").GetComponent<Button>();
            emailInput = trans.Find("Content/EmailInput").GetComponent<InputField>();
            errorText = trans.Find("Content/ErrorText").GetComponent<Text>();

            errorText.text = "";
            okBtn.onClick.AddListener(OnOkBtn);
            closeBtn.onClick.AddListener(OnCloseBtn);
        }

    }

    private void OnCloseBtn()
    {
        PanelMgr.instance.DestroyTip(this.GetType().ToString());
    }

    private async void OnOkBtn()
    {
        if (!PanelMgr.instance.NetConnect())
        {
            PanelMgr.instance.OpenTip<ErrorTip>("", "网络异常，请检查网络连接");
            return;
        }

        try
        {
            await AVUser.RequestPasswordResetAsync(emailInput.text);
            errorText.text="邮件已发送，请查收";
            StartCoroutine(ShowAndClose());
        }
        catch (Exception)
        {
            errorText.text = "邮件发送失败";
            StartCoroutine(ShowThenClear());
        } 
    }

    IEnumerator ShowThenClear()
    {
        yield return new WaitForSeconds(5f);
        errorText.text = "";
    }

    IEnumerator ShowAndClose()
    {
        yield return new WaitForSeconds(1.5f);
        OnCloseBtn();
    }
}