using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Connector connetor;

    private void Awake()
    {
        connetor = new Connector();
    }

    // Use this for initialization
    void Start () {
        connetor.Connect();

        connetor.Send(connetor._conn, "HelloWorld");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
