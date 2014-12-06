using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class Boot : MonoBehaviour{
   
    private string preSceneName;
    public Shader[] shaders;

    void Awake() {
        CameraManager.Main = Camera.main;
		App.init(this.gameObject, shaders);
        DontDestroyOnLoad(this.gameObject);
        UIManager.Instance.init(transform.Find("uiRoot"));
    }
    void Start() {
        createInputManager();
        
        
        LoadingManager.instance.load();
    }

    private void createInputManager(){
        App.input.init();
    }
    
    void OnLevelWasLoaded() {
        //dispose pre scene bundle
        App.bundle.disposePrevSceneBundle(preSceneName);
        preSceneName = Application.loadedLevelName;
        App.sceneManager.onSceneLoaded(Application.loadedLevelName);
        LoadingManager.instance.finishload(shaders);
    }

    void Update() {
        if(Application.isPlaying) App.update();
    }
    void OnGUI() {
       // GUILayout.Label()
    }

}
