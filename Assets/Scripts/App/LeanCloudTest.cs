using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using LeanCloud.Core.Internal;
using System.Threading.Tasks;
using LitJson;

public class LeanCloudTest : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Test();
        //ItemToJson();
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

        //账号名密码注册
        //AVUser user = new AVUser();
        //user.Username = "recho";
        //user.Password = "123456";

        //string id = "";

        ////手机号注册
        //var user = new AVUser();
        //user.Username = "WPUser";
        //user.Password = "avoscloud";
        //user.MobilePhoneNumber = "15639183221";
        //var task = user.SignUpAsync();
        //await task;
        //Debug.Log(AVUser.CurrentUser.Username);
        //user = await AVUser.LogInAsync("WPUser", "avoscloud");

        ////调用的前提是，该手机号已经与已存在的用户有关联(_User表中的mobilePhoneNumber即可关联手机，至于如何关联取决于客户端的业务逻辑)
        //await AVUser.RequestMobilePhoneVerifyAsync("18688888888").ContinueWith(t =>
        //{
        //    //这样就成功的发送了验证码
        //});

        //user.Username = "recho";
        //user.Password = "lichun";
        //user.Email = "1299395103@qq.com";
        //await user.SignUpAsync();

        //await AVUser.LogInAsync("recho", "lichun");
        //user = AVUser.CurrentUser;
        //Debug.Log(user.Username);

        //await AVUser.RequestEmailVerifyAsync(user.Email);
        //await AVUser.RequestPasswordResetAsync(user.Email);


        //AVObject football = new AVObject("Sport");
        //football["totalTime"] = 90;
        //football["name"] = "Football";
        //Task saveTask = football.SaveAsync();
        //await saveTask;


        //下载对象
        await AVUser.LogInAsync("recho", "lichun");

        AVObject obj = AVObject.CreateWithoutData("ObjModel", "5afbf116a22b9d004494b891");
        await obj.FetchAsync();

        string str = obj.Get<string>("modelInfo");
        Debug.Log(str);


        List<ModelData> jsonDatas = JsonMapper.ToObject<List<ModelData>>(str);

        for (int i = 0; i < jsonDatas.Count; i++)
        {
            ModelData data = jsonDatas[i];
            Debug.Log(data.AbUrl);
        }
    }

    public void ItemToJson()
    {
        List<ItemData> items = new List<ItemData>();
        ItemData item1 = new ItemData {
            Name="recho",
            Image="www.baidu.com",
            Obj="www.baidu.com",
            Video="www.baidu.com"
        };

        ItemData item2 = new ItemData
        {
            Name = "lichun",
            Image = "www.baidu.com",
            Obj = "www.baidu.com",
            Video = "www.baidu.com"
        };
        items.Add(item1);
        items.Add(item2);

        string jsonStr= JsonMapper.ToJson(items);
        Debug.Log(jsonStr);
    }
}

public class ItemData
{
    public string Name { get; set; }
    public string Image { get; set; }
    public string Obj { get; set; }
    public string Video { get; set; }
}

public class ModelData
{
    public string Name { get; set; }
    public string AbUrl { get; set; }
}
