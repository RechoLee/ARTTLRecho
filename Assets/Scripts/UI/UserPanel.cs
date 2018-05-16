using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserPanel:BasePanel
{

    private Button aboutBtn;
    private Button logoutBtn;

    private Text nameText;

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
            nameText = objTrans.Find("UserInfo/Text").GetComponent<Text>();

            aboutBtn.onClick.AddListener(OnAboutBtn);
            logoutBtn.onClick.AddListener(OnLogoutBtn);
        }
    }

    public override void OnShowed()
    {
        base.OnShowed();
        nameText.text = AVUser.CurrentUser.Username;
    }


    #endregion

    private async void OnLogoutBtn()
    {
        try
        {
            await AVUser.LogOutAsync();
            PanelMgr.instance.OpenPanel<LoginPanel>("");
        }
        catch (Exception)
        {
            PanelMgr.instance.OpenTip<ErrorTip>("","退出失败，请检查网络连接");
        }
    }

    private void OnAboutBtn()
    {
        PanelMgr.instance.OpenPanel<AboutPanel>("");
    }
}
