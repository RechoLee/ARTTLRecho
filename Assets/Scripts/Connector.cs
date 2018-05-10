using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Linq;

public class Connector : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Connect();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// socket连接
    /// </summary>
    public void Connect()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),1995);
        socket.Connect(iPEndPoint);

        string str = "HelloWorld";
        byte[] msgBytes = Encoding.UTF8.GetBytes(str);
        byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);
        byte[] sendBuff = lengthBytes.Concat(msgBytes).ToArray();
        socket.Send(sendBuff);
        Debug.Log("afdfadf");
    }
}
