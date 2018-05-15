using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel:BasePanel
{
    private Button backBtn;

    #region panel生命周期

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "AboutPanel";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(backBtn==null)
        {
            Transform trans = this.panelObj.transform;

            backBtn = trans.Find("Back/BackBtn").GetComponent<Button>();

            backBtn.onClick.AddListener(OnBackBtn);
        }
    }


    #endregion

    private void OnBackBtn()
    {
        PanelMgr.instance.OpenPanel<UserPanel>("");
    }

}