using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneBuilder : MonoBehaviour {
    private static string[] scenes = new string[] { "dungeon2" };



	 [MenuItem("Assets/sunny/buildScene-Android")]
    public static void buildScene_Android() {
        buildScene("android", BuildTarget.Android);
    }
     [MenuItem("Assets/sunny/buildScene-IOS")]
     public static void buildScene_IOS() {
         buildScene("ios", BuildTarget.iPhone);
     }
     [MenuItem("Assets/sunny/buildScene-Flash")]
     public static void buildScene_Flash() {
         buildScene("flash", BuildTarget.WebPlayer);
     }

    [MenuItem("Assets/sunny/buildScene-All")]
     public static void buildScene_All() {
         buildScene_Flash();
         buildScene_IOS();
         buildScene_Android();
     }
     private static void buildScene(string dir,BuildTarget target) {
         foreach (string scene in scenes) {
             string path = "Assets/bundle/"+dir+"/scene/" + scene + ".u3d";
             BuildPipeline.BuildPlayer(new string[] { "Assets/sunny/scenes/" + scene + ".unity" }, path, target, BuildOptions.BuildAdditionalStreamedScenes);
         }
     }
}
