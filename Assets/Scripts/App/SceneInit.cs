using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneInit : MonoBehaviour
{
    public ScreenOrientation screenOrientation;

    public PanelEnum defaultPanel;

    private void Awake()
    {
        Screen.orientation = screenOrientation;

    }

    // Use this for initialization
    void Start ()
    {
        switch (defaultPanel)
        {
            case PanelEnum.None:
                break;
            case PanelEnum.IndexPanel:
                PanelMgr.instance.OpenPanel<IndexPanel>("");
                break;
            case PanelEnum.ARPanel:
                PanelMgr.instance.OpenPanel<ARPanel>("");
                break;
            case PanelEnum.LoginPanel:
                PanelMgr.instance.OpenPanel<LoginPanel>("");
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        
	}
}

public enum Orientation
{
    Hori,
    Vert
}

/// <summary>
/// 面板枚举
/// </summary>
public enum PanelEnum
{
    None,
    IndexPanel,
    ARPanel,
    LoginPanel,
}
