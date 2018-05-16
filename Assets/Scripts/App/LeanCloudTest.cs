using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using LeanCloud.Core.Internal;
using System.Threading.Tasks;

public class LeanCloudTest : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Test();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public async void Test()
    {

        //AVClient.Initialize("MgmgluVuQ45nNvAJc4WlNbIs-gzGzoHsz", "MgmgluVuQ45nNvAJc4WlNbIs-gzGzoHsz");

        //AVObject obj = AVObject.Create("_User");
        //obj["username"] = "haha";
        //obj["password"] = "123456";

        AVUser user = new AVUser();
        user.Username = "recho";
        user.Password = "123456";
        string id="";
        await user.SignUpAsync().ContinueWith(t=> { id = user.ObjectId; });
        Debug.Log(id);

        AVObject football = new AVObject("Sport");
        football["totalTime"] = 90;
        football["name"] = "Football";
        Task saveTask = football.SaveAsync();
        await saveTask;
    }
}
