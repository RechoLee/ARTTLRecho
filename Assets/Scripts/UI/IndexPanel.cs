using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IndexPanel面板的操作方法，生命周期
/// </summary>
public class IndexPanel : BasePanel
{

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "IndexPanel";
        this.layer = PanelLayer.Panel;
    }
}
