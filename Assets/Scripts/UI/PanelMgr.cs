using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMgr : MonoBehaviour
{
    /// <summary>
    /// PanelMgr的单例
    /// </summary>
    public static PanelMgr instance;
    /// <summary>
    /// 场景中的canvas
    /// </summary>
    private GameObject canvas;

    /// <summary>
    /// 面板的字典
    /// </summary>
    private Dictionary<string, BasePanel> panelDict;

    /// <summary>
    /// 层级的root物体字典
    /// </summary>
    private Dictionary<PanelLayer, Transform> layerDict;

    #region Unity的方法

    private void Awake()
    {
        //单例
        instance = this;
        panelDict = new Dictionary<string, BasePanel>();
        layerDict = new Dictionary<PanelLayer, Transform>();
        Init();
    }



    #endregion

    #region 操作方法

    /// <summary>
    /// 初始化工作
    /// </summary>
    private void Init()
    {
        canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.LogError("canvas is null");
            return;
        }
        foreach (PanelLayer layer in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = layer.ToString();
            Transform layerTrans = canvas.transform.Find(name).transform;
            layerDict.Add(layer, layerTrans);
        }
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <typeparam name="T">面板类</typeparam>
    /// <param name="path">面板的prefab路径</param>
    /// <param name="args">初始化面板的参数</param>
    public void OpenPanel<T>(string path,params object[] args)
        where T:BasePanel
    {
        //判读是否已经打开
        string panelName = typeof(T).ToString();
        if (panelDict.ContainsKey(panelName))
            return;

        //添加之前判断
        BasePanel basePanel=canvas.GetComponent<T>();
        if (basePanel==null)
        {
            //添加面板脚本
            basePanel = canvas.AddComponent<T>();
            basePanel.Init(args);

            //从预制体中加载面板资源
            path = string.IsNullOrEmpty(path) ? basePanel.panelPath : path;
            GameObject panelObj = Resources.Load<GameObject>(path);
            if (panelObj == null)
            {
                Debug.LogError("panelObj is null");
                return;
            }
            basePanel.panelObj = Instantiate<GameObject>(panelObj);
            basePanel.panelObj.transform.SetParent(layerDict[basePanel.layer]);
        }
        else
        {
            basePanel.enabled = true;
            basePanel.panelObj.SetActive(true);
        }

        //调用生命周期函数
        basePanel.OnShowing();
        basePanel.OnShowed();

        panelDict.Add(panelName, basePanel);
    }

    /// <summary>
    /// 关闭面板操作
    /// </summary>
    /// <param name="panelName">面板名称</param>
    public void ClosePanel(string panelName)
    {
        BasePanel panel = panelDict[panelName];
        if (panel == null)
            return;

        //调用生命周期函数
        panel.OnClosing();
        panel.OnClosed();

        panelDict.Remove(panelName);
        panel.panelObj.SetActive(false);
        panel.enabled = false;
    }

    /// <summary>
    /// Destroy this panel
    /// </summary>
    /// <param name="panelName"></param>
    public void DestroyPanel(string panelName)
    {
        BasePanel panel = panelDict[panelName];
        if (panel == null)
            return;
        panel.OnClosing();
        panel.OnClosed();
        panelDict.Remove(panelName);
        GameObject.Destroy(panel.panelObj);
        Component.Destroy(panel);
    }

    #endregion
}
/// <summary>
/// 面板层级枚举
/// </summary>
public enum PanelLayer
{
    Panel,
    Tip
}

