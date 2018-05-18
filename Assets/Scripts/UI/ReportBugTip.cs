using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportBugTip :BasePanel
{
    private Button closeBtn;
    private Button okBtn;
    private Text errorText;
    private InputField userInput;


    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "ReportBugTip";
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
            errorText = trans.Find("Content/ErrorText").GetComponent<Text>();
            userInput = trans.Find("Content/UserInput").GetComponent<InputField>();

            okBtn.onClick.AddListener(OnOkBtn);
            closeBtn.onClick.AddListener(OnCloseBtn);
            errorText.text = "";

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
            errorText.text = "网络连接错误，请检查网络连接";
            StartCoroutine(ShowThenClear());
            return;
        }

        if (AVUser.CurrentUser==null)
        {
            errorText.text = "请先登录，方便我们采集错误信息";
            StartCoroutine(ShowThenClear());
            return;
        }
        AVObject obj = new AVObject("ReportBug");
        obj["reportUser"] =AVUser.CurrentUser.Username;
        if(userInput.text=="")
        {
            errorText.text = "请填写Bug信息";
            StartCoroutine(ShowThenClear());
            return;
        }
        obj["bugInfo"] = userInput.text;

        try
        {
            await obj.SaveAsync();
            errorText.text = "反馈成功,感谢你的反馈";
            StartCoroutine(ShowAndClose());
        }
        catch (Exception)
        {
            errorText.text = "网络连接错误，请检查网络连接";
            StartCoroutine(ShowThenClear());
            return;
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
