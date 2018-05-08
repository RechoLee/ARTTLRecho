using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 更改首页的图标样式和切换显示面板
/// </summary>
public class IndexIconChange : MonoBehaviour
{
    /// <summary>
    /// 首页界面上的三个按键
    /// </summary>
    private Button[] btnItems=new Button[3];

    private Button alreadyRegisBtn;
    private Button CreateNewBtn;

    [HideInInspector]
    //是否选中
    private bool[] isSelected;
    ///标题数组
    private string[] titles;
    private Text currText;
    private GameObject indexPanel;
    private GameObject loginPanel;
    private GameObject regisPanel;

    public Color originColor;
    public Color selectedColor;

    private void Awake()
    {
        isSelected = new bool[] { true, false, false };
        titles = new string[] {"ARTTL","AR","Me"};
        //找到几个item button
        btnItems[0] = GameObject.Find("Canvas/BG/Bottom-BG/Bottom-Item1").GetComponent<Button>();
        btnItems[1] = GameObject.Find("Canvas/BG/Bottom-BG/Bottom-Item2").GetComponent<Button>();
        btnItems[2] = GameObject.Find("Canvas/BG/Bottom-BG/Bottom-Item3").GetComponent<Button>();
        currText = GameObject.Find("Canvas/BG/Top-BG/Item-Title").GetComponent<Text>();
        loginPanel = GameObject.Find("Canvas/BG/Center/Login");
        indexPanel = GameObject.Find("Canvas/BG/Center/IndexPanel");
        regisPanel = GameObject.Find("Canvas/BG/Center/SignUp");
        alreadyRegisBtn = GameObject.Find("Canvas/BG/Center/SignUp/AlreadyRegisBtn").GetComponent<Button>();
        CreateNewBtn = GameObject.Find("Canvas/BG/Center/Login/CreateNewBtn").GetComponent<Button>();
    }

    private void Start()
    {
        //循环添加事件
        for (int i = 0; i < btnItems.Length; i++)
        {
            switch (i)
            {
                case (0):
                    btnItems[0].onClick.AddListener(OnBtnItem0);
                    break;
                case (1):
                    btnItems[1].onClick.AddListener(OnBtnItem1);
                    break;
                case (2):
                    btnItems[2].onClick.AddListener(OnBtnIte2);
                    break;
                default:
                    break;
            }
        }

        ///将为true的item 设置为被选中
        for (int i = 0; i < isSelected.Length; ++i)
        {
            if (isSelected[i])
            {
                currText.text = titles[i];
                btnItems[i].GetComponent<Image>().color = selectedColor;
                continue;
            }
            btnItems[i].GetComponent<Image>().color = originColor;
        }

        CreateNewBtn.onClick.AddListener(SwitchSignUp);
        alreadyRegisBtn.onClick.AddListener(SwitchLogin);
    }

    private void SwitchLogin()
    {
        regisPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private void SwitchSignUp()
    {
        loginPanel.SetActive(false);
        regisPanel.SetActive(true);
    }

    private void OnBtnItem0()
    {
        for (int i = 0; i < isSelected.Length; i++)
        {
            if (isSelected[i])
            {
                btnItems[i].GetComponent<Image>().color = originColor;
            }
            isSelected[i]= false;
        }
        loginPanel.SetActive(false);
        regisPanel.SetActive(false);
        indexPanel.SetActive(true);
        currText.text= titles[0];
        isSelected[0] = true;
        btnItems[0].GetComponent<Image>().color = selectedColor;
    }

    private void OnBtnItem1()
    {
        for (int i = 0; i < isSelected.Length; i++)
        {
            if (isSelected[i])
            {
                btnItems[i].GetComponent<Image>().color = originColor;
            }
            isSelected[i] = false;
        }
        loginPanel.SetActive(false);
        indexPanel.SetActive(false);
        regisPanel.SetActive(false);
        currText.text = titles[1];
        isSelected[1] = true;
        btnItems[1].GetComponent<Image>().color = selectedColor;
    }

    private void OnBtnIte2()
    {
        for (int i = 0; i < isSelected.Length; i++)
        {
            if (isSelected[i])
            {
                btnItems[i].GetComponent<Image>().color = originColor;
            }
            isSelected[i] = false;
        }
        loginPanel.SetActive(true);
        indexPanel.SetActive(false);
        currText.text = titles[2];
        isSelected[2] = true;
        btnItems[2].GetComponent<Image>().color = selectedColor;
    }

}
