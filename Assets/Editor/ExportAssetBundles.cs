using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using editor;

public class ExportAssetBundles 
{
	static int lastVersion = 0;
    static StreamWriter logWriter = new StreamWriter("Assets/Output/log.txt");
    const string SCENE_WIN_PATH = "/Output/win_scenes/";
    const string SCENE_IOS_PATH = "/Output/ios_scenes/";
    const string SCENE_ANDROID_PATH = "/Output/android_scenes/";
    const string PREFAB_WIN_PATH = "/Output/win_prefabs/";
    const string PREFAB_IOS_PATH = "/Output/ios_prefabs/";
    const string PREFAB_ANDROID_PATH = "/Output/android_prefabs/";

    [MenuItem("Create Bundle/Save Win ICON")]
    static void ExportWinIcon()
    {
        ExportIcon(BuildTarget.WebPlayer);
    }

    [MenuItem("Create Bundle/Save IOS ICON")]
    static void ExportIOSICon()
    {
        ExportIcon(BuildTarget.iPhone);
    }

    [MenuItem("Create Bundle/Save Android ICON")]
    static void ExportAndroidICon()
    {
        ExportIcon(BuildTarget.Android);
    }

    [MenuItem("Create Bundle/Save Win Bundle")]
    static void ExportWinBundle()
    {
        ExportBundle(BuildTarget.WebPlayer);
    }

    [MenuItem("Create Bundle/Save IOS Bundle")]
    static void ExportIOSBundle()
    {
       ExportBundle(BuildTarget.iPhone);
    }

    [MenuItem("Create Bundle/Save Android Bundle")]
    static void ExportAndroidBundle()
    {
        ExportBundle(BuildTarget.Android);
    }  

    [MenuItem("Create Bundle/Save Win Scene")]
	static void ExportWinScene()
    {
		ExportScenes(BuildTarget.WebPlayer, SCENE_WIN_PATH);
	}

    [MenuItem("Create Bundle/Save IOS Scene")]
    static void ExportIOSScene()
    {
        ExportScenes(BuildTarget.iPhone, SCENE_IOS_PATH);
    }

    [MenuItem("Create Bundle/Save Android Scene")]
    static void ExportAndroidScene()
    {
        ExportScenes(BuildTarget.Android, SCENE_ANDROID_PATH);
    }

    [MenuItem("Create Bundle/Save Win Prefabs")]
	static void ExportWinPrefabs()
	{
       ExportPrefabs(BuildTarget.WebPlayer, PREFAB_WIN_PATH);
	}

    [MenuItem("Create Bundle/Save IOS Prefabs")]
    static void ExportIOSPrefabs()
    {
        ExportPrefabs(BuildTarget.iPhone, PREFAB_IOS_PATH);
    }

    [MenuItem("Create Bundle/Save Android Prefabs")]
    static void ExportAndroidPrefabs()
    {
        ExportPrefabs(BuildTarget.Android, PREFAB_ANDROID_PATH);
    }

    static void ExportIcon(BuildTarget type)
    {
        Caching.CleanCache();
        string path = Application.dataPath + "/StreamingAssets/picture.u3d";
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }
        BuildPipeline.BuildAssetBundle(null, selectAssets, path, options, type);
    }

    static void ExportBundle(BuildTarget type)
    {
        Caching.CleanCache();
        string prefix = Application.dataPath + "/StreamingAssets/";
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies;
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets) {
            GameObject go = obj as GameObject;
            if (go) {
                Debug.Log("Create AssetBunldes name :" + go.name);
                string path = prefix + go.name + ".u3d";
                BuildPipeline.BuildAssetBundle(null, selectAssets, path, options, type);
            }          
        }
        
    }

    static void ExportScenes(BuildTarget type, string path)
    {
        Caching.CleanCache();
        ReadBundle();
        List<BundleItem> items = BundleConfig.Instance.getBundleItemByType(EBundleType.eBundleScene);
        for (int i = 0; i < items.Count; i++)
        {
            BundleItem item = items[i];
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;
            string[] buildArr = new string[item.subPrefabs.Count];
            for (int j = 0; j < item.subPrefabs.Count; j++)
            {
                buildArr[j] = item.subPrefabs[j] + ".unity";
            }
            string outFile = Application.dataPath + path + item.bundleName + ".u3d";
            BuildPipeline.BuildPlayer(buildArr, outFile, type, BuildOptions.BuildAdditionalStreamedScenes);
        }
    }

    static void ExportPrefabs(BuildTarget type, string path)
    {
        string bundlePrefix = "Assets/Resources/";
        ReadBundle();
        bool success;
        string prefabName;
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
        //List<BundleItem> items = BundleConfig.Instance.getBundleItemByType(EBundleType.eBundleShader);
        //BundleItem shaderItem = items[0];
        ////BuildPipeline.PushAssetDependencies();
        //Object shaderObj = AssetDatabase.LoadMainAssetAtPath(bundlePrefix + shaderItem.subPrefabs[0] + ".prefab");
        //prefabName = Application.dataPath + PREFAB_IOS_PATH + shaderItem.bundleName + ".u3d";
        //BuildPipeline.BuildAssetBundle(shaderObj, null, prefabName, options | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.iPhone);
        foreach (BundleItem item in BundleConfig.Instance.AllBundleItems)
        {
            if (item.bundleType == EBundleType.eBundleShader || item.bundleType == EBundleType.eBundleScene) //忽略场景的bundle
                continue;
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;
            int len = item.subPrefabs.Count;
            Object[] childObjs = new Object[len];
            for (int i = 0; i < len; i++)
            {
                string objPath = null;
                if (item.bundleType == EBundleType.eBundleMusic)
                    objPath = bundlePrefix + item.subPrefabs[i];
                else
                    objPath = bundlePrefix + item.subPrefabs[i] + ".prefab";
                childObjs[i] = AssetDatabase.LoadMainAssetAtPath(objPath);
                if (childObjs[i] == null)
                {
                    Debug.LogError("The asset: <" + item.bundleName + "> sub prefabs <" + item.subPrefabs[i] + "> not exist");
                    return;
                }
            }
            //BuildPipeline.PushAssetDependencies();
            prefabName = Application.dataPath + path + item.bundleName + ".u3d";
            success = BuildPipeline.BuildAssetBundle(null, childObjs, prefabName, options, type);
            if (!success)
            {
                Debug.LogError("BuildAsset: <" + item.bundleName + "> error!");
                logWriter.WriteLine("BuildAsset: <" + item.bundleName + "> error!");
            }
            //BuildPipeline.PopAssetDependencies();
        }
        //BuildPipeline.PopAssetDependencies();
        logWriter.Flush();
    }

    static bool FindShader(GameObject obj, string bundleName, string prefabName)
    {
        bool notExist = false;
        foreach (Transform trans in obj.transform)
        {
            GameObject childObj = trans.gameObject;
            if(childObj.GetComponent<Renderer>() != null)
            {
                Renderer renderer = childObj.GetComponent<Renderer>();
				if (renderer.material != null)
				{
				    Debug.Log(renderer.material.shader.name);
                    if (renderer.material.shader.name.IndexOf("FX PACK 1") != -1)
                    {
                        Debug.Log("The asset: <" + bundleName + "> sub prefabs <" + prefabName + "> use error shader");
                        return false;;
                    }
                    else
                    {
                        notExist = FindShader(childObj, bundleName, prefabName);
                        if(!notExist)
                            break;
                    }
                }
            }
            
        }
        return notExist;
    }

    static void ReadBundle()
    {
        BundleConfig.Instance.path = "Local/data/bundle";
        TextAsset ta = Resources.Load(BundleConfig.Instance.path) as TextAsset;
        if (ta)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ta.text);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode(BundleConfig.Instance.RootName).ChildNodes;
            for (int k = 0; k < nodeList.Count; k++)
            {
                XmlElement xe = nodeList.Item(k) as XmlElement;
                if (xe == null)
                    continue;
				if(k == 0)
				{
					lastVersion = int.Parse(xe.GetAttribute("lastVersion"));
					continue;
				}
                string key = xe.GetAttribute("ID");
                for (int i = 0; i < xe.Attributes.Count; i++)
                {
                    XmlAttribute attr = xe.Attributes[i];
                    BundleConfig.Instance.appendAttribute(int.Parse(key), attr.Name, attr.Value);
                }
            }			
        }
    }
}
