using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFirstTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
        EasyTouch.On_Drag += EasyTouch_On_Drag;
        Debug.Log("Register");
    }

    private void EasyTouch_On_Drag(Gesture gesture)
    {
        throw new System.NotImplementedException();
    }

    private void EasyTouch_On_TouchStart(Gesture gesture)
    {
        Debug.Log(gesture.position);
        if(gesture.pickedObject==this.gameObject)
        {
            Debug.Log("zzz");
        }
    }

    private void OnDisable()
    {
        EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
        
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
    }

    public void MyFunc()
    {
        Debug.Log("hahah");
    }
}
