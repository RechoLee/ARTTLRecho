using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 面板prefab的路径
    /// </summary>
    public string panelPath;
    /// <summary>
    /// 面板实例化的GameObject对象
    /// </summary>
    public GameObject panelObj;
    /// <summary>
    /// 面板所属的类型层级
    /// </summary>
    public PanelLayer layer;
    /// <summary>
    /// 面板相关的参数
    /// </summary>
    public object[] args;

    #region 面板的生命周期函数

    /// <summary>
    /// 面板初始化
    /// </summary>
    /// <param name="_args">初始化的参数</param>
    public virtual void Init(params object[] _args)
    {
        this.args = _args;
    }

    /// <summary>
    /// 面板正在显示 
    /// </summary>
    public virtual void OnShowing() { }

    /// <summary>
    /// 面板已经显示后
    /// </summary>
    public virtual void OnShowed() { }

    /// <summary>
    /// 面板更新
    /// </summary>
    public virtual void Update() { }
    
    /// <summary>
    /// 面板正在关闭
    /// </summary>
    public virtual void OnClosing() { }

    /// <summary>
    /// 面板已经关闭
    /// </summary>
    public virtual void OnClosed() { }

    #endregion

    #region 面板的操作方法

    /// <summary>
    /// 关闭操作
    /// </summary>
    public virtual void Close()
    {
        string panelName = this.GetType().ToString();
        PanelMgr.instance.ClosePanel(panelName);
    }
    #endregion
}