using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class Boot : MonoBehaviour{
   
    private string preSceneName;
    public Shader[] shaders;

    void Awake() {
        App.init(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    void Start() {
        createInputManager();
        UIManager.Instance.init(transform.Find("uiRoot"));
        
        LoadingManager.instance.load();
    }

    private void createInputManager(){
        App.input.init();
    }
    
    void OnLevelWasLoaded() {
        //dispose pre scene bundle
        App.bundle.disposePrevSceneBundle(preSceneName);
        preSceneName = Application.loadedLevelName;
        CameraManager.Main = Camera.main;
        App.sceneManager.onSceneLoaded(Application.loadedLevelName);
        LoadingManager.instance.finishload();
    }

    void Update() {
        if(Application.isPlaying) App.update();
    }
    void OnGUI() {
       // GUILayout.Label()
    }

}
