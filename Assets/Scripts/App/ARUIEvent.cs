using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARUIEvent : MonoBehaviour {

    private Button backBtn;

    private void Awake()
    {
        if (GameObject.Find("Canvas/BackBtn"))
        {
            backBtn = GameObject.Find("Canvas/BackBtn").GetComponent<Button>();
        }
    }

    // Use this for initialization
    void Start () {
        if(backBtn!=null)
            backBtn.onClick.AddListener(OnBackBtn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 点击返回键 处理返回场景前的工作和切换场景
    /// </summary>
    private void OnBackBtn()
    {
        AsyncOperation result = SceneManager.LoadSceneAsync("main");
        PanelMgr.instance.OpenPanel<LoadingPanel>("",result);
    }
}
