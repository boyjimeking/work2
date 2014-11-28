using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[InitializeOnLoad]
public class ShadowSoftenerUpdate
{
	const string BasePath = "Assets/Shadow Softener";
	
    static ShadowSoftenerUpdate()
    {
		DeleteIfExists("Readme.txt");
		DeleteIfExists("Readme.html");
    }
	
	static void DeleteIfExists(string relativePath)
	{
		string assetPath = string.Format("{0}/{1}", BasePath, relativePath);
		if(File.Exists(assetPath))
			AssetDatabase.DeleteAsset(assetPath);
	}
}
