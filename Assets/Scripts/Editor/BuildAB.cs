using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildAB : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [MenuItem("AB/Android")]
    public static void BuildAndroid()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("AB/Win")]
    public static void BuildWin()
    {
        Build(BuildTarget.StandaloneWindows64);
    }

    public static void Build(BuildTarget target)
    {
        string path = $"{Application.streamingAssetsPath}";

        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                path += "/Win";
                break;
            case BuildTarget.Android:
                path += "/Android";
                break;
            default:
                break;
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, target);

    }
}
