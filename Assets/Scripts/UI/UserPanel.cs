using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel:BasePanel
{

    private Button aboutBtn;
    private Button logoutBtn;

    #region 面板生命周期

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "UserPanel";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(aboutBtn==null&&logoutBtn==null)
        {
            Transform objTrans = this.panelObj.transform;
            aboutBtn = objTrans.Find("About/BG").GetComponent<Button>();
            logoutBtn = objTrans.Find("Logout/BG").GetComponent<Button>();

            aboutBtn.onClick.AddListener(OnAboutBtn);
            logoutBtn.onClick.AddListener(OnLogoutBtn);
        }
    }


    #endregion

    private void OnLogoutBtn()
    {
        PanelMgr.instance.OpenPanel<LoginPanel>("");
    }

    private void OnAboutBtn()
    {
        PanelMgr.instance.OpenPanel<AboutPanel>("");
    }
}
