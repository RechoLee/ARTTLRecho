using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{

    private Image bar;

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "LoadingPanel";
        this.layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if (bar == null)
        {
            Transform trans = this.panelObj.transform;
            bar = trans.Find("Load/LoadingBar/Bar").GetComponent<Image>();
        }

    }

    public override void OnShowed()
    {
        base.OnShowed();

        if(args[0]!=null)
        {
            AsyncOperation result = args[0] as AsyncOperation;
            bar.fillAmount = result.progress;
        }
    }
}
