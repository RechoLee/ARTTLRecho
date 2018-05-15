using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorTip : BasePanel
{
    private Button closeBtn;

    private Text errorText;

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "ErrorTip";
        this.layer = PanelLayer.Tip;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(closeBtn==null&&errorText==null)
        {
            Transform trans = this.panelObj.transform;
            closeBtn = trans.Find("Content/CloseBtn").GetComponent<Button>();
            errorText = trans.Find("Content/ErrorText").GetComponent<Text>();

            closeBtn.onClick.AddListener(OnCloseBtn);
        }
    }

    public override void OnShowed()
    {
        base.OnShowed();
        if(this.args!=null)
        {
            errorText.text= this.args[0] as string;
        }
    }

    private void OnCloseBtn()
    {
        PanelMgr.instance.CloseTip(typeof(ErrorTip).ToString());
    }
}
