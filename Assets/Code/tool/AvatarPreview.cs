using UnityEngine;
using System.Collections;
using UnityEditor;


public class AvatarPreview : MonoBehaviour {
    private string[] animNames;
    private Animator animator;

    public GameObject fbx;
    public GameObject effectPrefab;
    public float effectLife = 2f;
   
    void Start() {
       
       string path= AssetDatabase.GetAssetPath(fbx);
       ModelImporter mi = AssetImporter.GetAtPath(path) as ModelImporter;
       ModelImporterClipAnimation[] anims = mi.clipAnimations;
       animNames = new string[anims.Length];
       for (int i = 0; i < anims.Length; i++) {
           animNames[i] = anims[i].name;
       }

       animator = gameObject.GetComponent<Animator>();
       //animator.runtimeAnimatorController = null;
       animator.Play(animNames[0],0,0);

    }
    void OnGUI() {
        if (animNames==null) return;
        GUILayout.BeginVertical();
        foreach (string name in animNames) {
            if (GUILayout.Button(name)) {
                animator.Play(name,0,0);
                if (effectPrefab != null) {
                   GameObject effect= GameObject.Instantiate(effectPrefab) as GameObject;
                   GameObject.Destroy(effect, effectLife);
                }
            }
        }
        GUILayout.EndVertical();
    }

    // Update is called once per frame
    void Update() {

    }
    void NewEvent(AnimationEvent e) {

    }
}
