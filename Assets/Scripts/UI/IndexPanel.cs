using LeanCloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// IndexPanel面板的操作方法，生命周期
/// </summary>
public class IndexPanel : BasePanel
{
    private List<ModelData> modelDatas;

    private Button searchBtn;
    private InputField searchInput;
    private Transform content;
    private Scrollbar scrollBar;
    private Text bottomText;
    private Text topText;
    private Image loadingImg;

    private GameObject itemObj;

    private bool isFirst = true;

    private float progressCount = 0;
    private float progress = 0;

    private bool canUpdate = true;

    private float lastSize;

    public override void Init(params object[] _args)
    {
        base.Init(_args);
        this.panelPath = "IndexPanel";
        this.layer = PanelLayer.Panel;

        if(itemObj==null)
        {
            itemObj= Resources.Load<GameObject>("Item");
        }
    }

    public override void OnShowing()
    {
        base.OnShowing();

        if(searchBtn==null&&content==null)
        {
            Transform trans = this.panelObj.transform;
            searchBtn = trans.Find("Top/Search/Icon").GetComponent<Button>();
            searchInput = trans.Find("Top/Search/InputField").GetComponent<InputField>();
            scrollBar = trans.Find("ScrollRect/Scrollbar").GetComponent<Scrollbar>();
            bottomText = trans.Find("ScrollRect/BottomText").GetComponent<Text>();
            topText = trans.Find("ScrollRect/TopText").GetComponent<Text>();
            content = trans.Find("ScrollRect/Content");
            loadingImg = trans.Find("Top/Loading").GetComponent<Image>();

            searchBtn.onClick.AddListener(OnSearchBtn);
        }

        topText.gameObject.SetActive(false);
        bottomText.gameObject.SetActive(false);
        loadingImg.gameObject.SetActive(false);
    }

    public override void OnShowed()
    {
        base.OnShowed();
        canUpdate = true;
        if (isFirst)
        {
            UpdateResources();
        }
    }

    public override void Update()
    {

        base.Update();

        ///用户下拉上拉
        ScrollBarChange();
    }

    /// <summary>
    /// 更新资源
    /// </summary>
    public async void UpdateResources()
    {
        if(!PanelMgr.instance.NetConnect())
        {
            PanelMgr.instance.OpenTip<ErrorTip>("","网络异常，请检查网络连接");
            return;
        }

        if(AVUser.CurrentUser==null)
        {
            PanelMgr.instance.OpenTip<ErrorTip>("","请登录获取更多资源");
            return;
        }

        AVUser user = AVUser.CurrentUser;
        AVObject obj = AVObject.CreateWithoutData("ObjModel", "5afbf116a22b9d004494b891");
        try
        {
            await obj.FetchAsync();
            string jsonStr=obj.Get<string>("modelInfo");
            List<ModelData> models = JsonMapper.ToObject<List<ModelData>>(jsonStr);

            if(isFirst)
            {
                modelDatas = models;
                if (models != null)
                {
                    //设置加载进度
                    progress = models.Count;
                    progressCount = progress;
                    //设置Content大小
                    content.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(
                        content.GetComponent<RectTransform>().sizeDelta.x, 240 * models.Count);

                    for (int i = 0; i < models.Count; i++)
                    {
                        StartCoroutine(CreateItem(models[i]));
                    }
                    isFirst = false;
                }
            }
            else
            {

                Debug.Log(1);
                var result = models.Except<ModelData>(modelDatas, new ModelDataCompare()).ToArray();
                if (result.Length > 0)
                {
                    Debug.Log(2);
                    //设置加载进度
                    progress = models.Count;
                    progressCount = progress;
                    //设置Content大小
                    content.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(
                        content.GetComponent<RectTransform>().sizeDelta.x, 240 * (modelDatas.Count + result.Length));

                    modelDatas = models;

                    for (int i = 0; i < result.Length; i++)
                    {
                        StartCoroutine(CreateItem(result[i]));
                    }
                }
            }

            lastSize = scrollBar.size;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            PanelMgr.instance.OpenTip<ErrorTip>("","更新资源失败");
            return;
        }
    }


    /// <summary>
    /// 创建Item
    /// </summary>
    IEnumerator CreateItem(ModelData data)
    {
        Transform trans=GameObject.Instantiate(itemObj,content).transform;

        trans.Find("Name").GetComponent<Text>().text=data.Name;
        Button btn= trans.Find("OpenBtn").GetComponent<Button>();
        btn.onClick.AddListener(OnOpenBtn);
        Image img = trans.Find("Image").GetComponent<Image>();

        WWW www = new WWW(data.ImgUrl);
        yield return www;
        progress -= www.progress;

        //将下载的图片映射到sprite 后续可能回用到
        Texture2D texture2d = www.texture;
        img.sprite= Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    }

    private void OnOpenBtn()
    {

    }




    /// <summary>
    /// 检测用户的下拉上拉操作 和下载的progress
    /// </summary>
    public void ScrollBarChange()
    {
        if (scrollBar.value == 1)
        {
            if (OnSizeChanged())
            {
                if (!topText.gameObject.activeSelf)
                {
                    topText.gameObject.SetActive(true);
                    //控制 连续更新周期不能小于两秒
                    if (canUpdate)
                    {
                        canUpdate = false;
                        UpdateResources();
                        StartCoroutine(ResetCanUpdate());
                    }
                }
            }
            else
            {
                if (topText.gameObject.activeSelf)
                {
                    topText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (topText.gameObject.activeSelf)
            {
                topText.gameObject.SetActive(false);
            }
        }
        if (scrollBar.value == 0)
        {
            if (OnSizeChanged())
            {
                if (!bottomText.gameObject.activeSelf)
                {
                    bottomText.gameObject.SetActive(true);
                }
            }
            else
            {
                if (bottomText.gameObject.activeSelf)
                {
                    bottomText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (bottomText.gameObject.activeSelf)
            {
                bottomText.gameObject.SetActive(false);
            }
        }

        if (loadingImg.fillAmount == 1)
        {
            if (loadingImg.gameObject.activeSelf)
                loadingImg.gameObject.SetActive(false);
        }
        else
        {
            if (progressCount != 0)
            {
                loadingImg.fillAmount = progress / progressCount;
                if (!loadingImg.gameObject.activeSelf)
                {
                    loadingImg.gameObject.SetActive(true);
                }
            }
        }
    }

    private IEnumerator ResetCanUpdate()
    {
        yield return new WaitForSeconds(2f);
        canUpdate = true;
    }

    /// <summary>
    /// 判断用户下拉 上拉
    /// </summary>
    /// <returns></returns>
    private bool OnSizeChanged()
    {
        if(Math.Abs(lastSize-scrollBar.size)>0.05f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnSearchBtn()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// model 数据
/// </summary>
public class ModelData
{
    public string Name { get; set; }
    public string AbUrl { get; set; }
    public string ImgUrl { get; set; }
}


/// <summary>
/// 比较ModelData
/// </summary>
public class ModelDataCompare : IEqualityComparer<ModelData>
{
    public bool Equals(ModelData x, ModelData y)
    {
        return (x.Name==y.Name&&x.AbUrl==y.AbUrl&&x.ImgUrl==y.ImgUrl);
    }

    public int GetHashCode(ModelData obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            return obj.ToString().GetHashCode();
        }
    }
}
