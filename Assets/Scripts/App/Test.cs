﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public InputField ShowText;
    public InputField input;
    public Button send;

    public Connector connetor;

    private void Awake()
    {
        connetor = new Connector();
    }

    // Use this for initialization
    void Start () {

        connetor.Connect();
    }
	
    public void OnClickSend()
    {
        string msg=input.text;

        BytesProtocol proto = new BytesProtocol();

        //proto.AddString("Register");
        //proto.AddString("lijinrun");
        //proto.AddString("123456");
        //proto.AddString("12154545@qq.com");

        proto.AddString("Login");
        proto.AddString("lijinrun");
        proto.AddString("123456");

    }

    // Update is called once per frame
    void Update () {
    }

}