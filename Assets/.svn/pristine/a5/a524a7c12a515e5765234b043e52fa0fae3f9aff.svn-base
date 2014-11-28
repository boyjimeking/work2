using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


public class AvatarPreview : MonoBehaviour {
    private string[] animNames;
    private Animator animator;


    public GameObject effectPrefab;
    public float effectLife = 2f;
    private Animation anim;

    private GameObject fbx;
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        if (animator == null) {
            anim = gameObject.GetComponent<Animation>();
           int count= anim.GetClipCount();
           animNames = new string[count];
           int index = 0;

           foreach (AnimationState clip in anim) {
               animNames[index++]=clip.name;
           }
           anim.Play(animNames[0]);
          
        } else {
            string path = AssetDatabase.GetAssetPath(fbx);
            ModelImporter mi = AssetImporter.GetAtPath(path) as ModelImporter;
            ModelImporterClipAnimation[] anims = mi.clipAnimations;
            animNames = new string[anims.Length];
            for (int i = 0; i < anims.Length; i++) {
                animNames[i] = anims[i].name;
            }
            animator.Play(animNames[0], 0, 0);
        }

    }
    void OnGUI() {
        if (animNames==null) return;
        GUILayout.BeginVertical();
        foreach (string name in animNames) {
            if (GUILayout.Button(name)) {
                if (animator != null) {
                    animator.Play(name, 0, 0);
                } else {
                    anim.Play(name);
                }
                
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
#endif
