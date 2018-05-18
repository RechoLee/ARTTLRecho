using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 应用场景中的类
/// </summary>
public class Root : MonoBehaviour
{


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //通信层的更新
        NetMgr.Update();	
	}
}
