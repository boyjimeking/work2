using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class ShadowSoftenerConverter : EditorWindow
{
	public enum FilterNames
	{
		BuiltIn,
		PCF2x2,
		PCF3x3,
		PCF4x4,
		PCF5x5,
		PCF6x6,
		PCF7x7,
		PCF8x8,
	}
	FilterNames FilterName = FilterNames.PCF4x4;
	bool CreateBackup = true;
	
	static ShadowSoftenerConverter dialogWin;
	[MenuItem ("Tools/Shadow Softener/Convert Shaders")]
	static void ConvertShaders()
	{
		if(!dialogWin)
			dialogWin = ScriptableObject.CreateInstance<ShadowSoftenerConverter>();
		dialogWin.title = "Shadow Softener Shader Converter";
		dialogWin.ShowUtility();
	}
	void OnGUI()
	{
		int shaderCount = 0;
		foreach(var obj in Selection.objects)
		{
			if(obj is Shader)
				shaderCount++;
		}
		
		if(shaderCount <= 0)
		{
			EditorGUILayout.HelpBox("Please select one or more Shaders from your Project", MessageType.Info);
		}
		else
		{
			EditorGUILayout.HelpBox(string.Format("{0} Shader{1} Selected", shaderCount, (shaderCount>1)?"s":""), MessageType.None);
			CreateBackup = EditorGUILayout.Toggle("Create Backup", CreateBackup);
			FilterName = (FilterNames)EditorGUILayout.EnumPopup("Filter Type", FilterName);
			if(GUILayout.Button("Convert"))
			{
				GenerateSofterShaders();
				Close();
			}	
		}
	}
	
	void GenerateSofterShaders ()
	{
		bool needsRefresh = false;
		foreach(var obj in Selection.objects)
		{
			Shader shader = obj as Shader;
			if(shader == null)
				continue;
			string path = AssetDatabase.GetAssetPath(shader);
			if(path == null)
				continue;
			var code = File.ReadAllText(path);
			var matches = SubShaderMatches(code);
			if(matches.Count <= 0)
			{
				Debug.Log(string.Format("Unable to Convert Shader: {0}", path));
				break;
			}
			code = RemoveUsePasses(code);
			if(FilterName != FilterNames.BuiltIn)
				code = AddUsePasses(code, FilterName);
			if(CreateBackup)
				File.Copy(path, AssetDatabase.GenerateUniqueAssetPath(path+".backup"));
			File.WriteAllText(path, code);
			needsRefresh = true;
		}
		if(needsRefresh)
			AssetDatabase.Refresh();
	}
	
	static string SubShaderSearchString = @"(\s*SubShader\s*\{)";
	static MatchCollection SubShaderMatches(string code)
	{
		return Regex.Matches(code, SubShaderSearchString);
	}
	static string AddUsePasses(string code, FilterNames filter)
	{
		// Question mark makes it non-greedy....
		// Matching: UsePass\s*\"ShadowSoftener.*?\"
		return Regex.Replace(code, SubShaderSearchString, string.Format("$1\n\tUsePass \"ShadowSoftener/{0}\"", filter.ToString().ToUpper()));
	}
	static string RemoveUsePasses(string code)
	{
		// Question mark makes it non-greedy....
		// Matching: UsePass\s*\"ShadowSoftener.*?\"
		return Regex.Replace(code, "\\s*UsePass\\s*\\\"ShadowSoftener.*?\\\"", "");
	}
}
