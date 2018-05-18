using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackBtn : MonoBehaviour {

    Button backBtn;

    private void Awake()
    {
        backBtn=GetComponent<Button>();
    }

    // Use this for initialization
    void Start ()
    {
        backBtn.onClick.AddListener(OnBackBtn);
        Debug.Log(PlayerPrefs.GetString("ModelName"));
    }

    /// <summary>
    /// 点击返回键 处理返回场景前的工作和切换场景
    /// </summary>
    private void OnBackBtn()
    {
        AsyncOperation result = SceneManager.LoadSceneAsync("main");
        PanelMgr.instance.OpenPanel<LoadingPanel>("", result);
    }


}
