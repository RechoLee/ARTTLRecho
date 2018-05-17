using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel:BasePanel
{
    private Button backBtn;

    private Text aboutText;

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
            aboutText = trans.Find("AboutText/Text").GetComponent<Text>();

            backBtn.onClick.AddListener(OnBackBtn);
        }
    }

    public async override void OnShowed()
    {
        base.OnShowed();

        if(AVUser.CurrentUser==null)
        {
            return;
        }
        AVObject obj = AVObject.CreateWithoutData("AboutApp", "5afcf8c99f545452b2c65994");
        try
        {
            await obj.FetchAsync();
            aboutText.text = obj.Get<string>("aboutInfo");
        }
        catch (Exception)
        {
            return;
        }

    }

    #endregion

    private void OnBackBtn()
    {
        PanelMgr.instance.OpenPanel<UserPanel>("");
    }

}