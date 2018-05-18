using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// AR面板
/// </summary>
public class ARPanel:BasePanel
{
    private Image bar;

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "ARPanel";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(bar==null)
        {
            Transform trans = this.panelObj.transform;
            bar = trans.Find("Load/LoadingBar/Bar").GetComponent<Image>();
        }


    }

    public override void OnShowed()
    {
        base.OnShowed();

        //打开AR场景
        AsyncOperation result= SceneManager.LoadSceneAsync("ar");

        bar.fillAmount = result.progress;
    }


}