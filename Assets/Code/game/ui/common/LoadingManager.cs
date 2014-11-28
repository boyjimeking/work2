using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;

public class LoadingManager : IProgressListener{

    public enum LoadingState
    {
        bundle, xml, prefab, scene
    }

    public static LoadingManager instance = new LoadingManager();

    public delegate void loadXmlCallBack();
    public GameObject go;
    public UILabel tip;
    private UISprite run, rungbl, progress;
    private LoadingState state;
    private string statestr;
    private AsyncOperation async;
    public bool oncheck,loaded;

    public void init(){
        if (go == null){
            go = Engine.res.createSingle("Local/prefab/loadingPanel");
            go.SetActive(false);
            GameObject gress = go.getChild("progress");
            progress = gress.getChildComponent<UISprite>("progress");
            run = gress.getChildComponent<UISprite>("run");
            rungbl = gress.getChildComponent<UISprite>("rungbl");
            tip = go.getChildComponent<UILabel>("tip");
        }
        onProgress(0, 1);
        go.SetActive(true);
        //go.transform.localScale = new Vector3(Screen.width/960f,Screen.height/640f,1);
    }

    public void showtip(LoadingState _state){
        init();
        state = _state;
        statestr = "";
        switch(state){
            case LoadingState.bundle:{
                statestr = "加载bundle中... ";
                checkBundle();
                break;
            }
            case LoadingState.xml:{
                statestr = "加载xml中... ";
                break;
            }
            case LoadingState.prefab:{
                statestr = "加载prefab中... ";
                checkUsePrefab();
                break;
            }
            case LoadingState.scene:{
                statestr = "加载scene中... ";
                break;
            }
        }
        tip.text = statestr + "0.00%";
        onProgress(0, 1);
    }
    public void onProgress(int totalCount, int completedCount, float currentBundleProgress) {
        onProgress(1.0f * completedCount / totalCount,currentBundleProgress);
    }
    public void onProgress(float currentProgress, float currentBundleProgress) {
        if (currentProgress < 0) currentProgress = 0;
        if (currentProgress > 1) currentProgress = 1;
        
            progress.fillAmount = currentProgress;
            tip.text = statestr + string.Format("{0:F2}", progress.fillAmount*100) + "%";
            run.transform.localPosition = new Vector3(-400 + progress.fillAmount * 700, run.transform.localPosition.y, run.transform.localPosition.z);
            rungbl.transform.localPosition = new Vector3(-360 + progress.fillAmount * 700, rungbl.transform.localPosition.y, rungbl.transform.localPosition.z);
    }

    public void onComplete() {
        
    }

    public void checkBundle()
    {

        BundleManager dm = App.bundle;
        //test download and load scene
        //dm.baseURL = baseURL=Application.dataPath + "/../AssetBundles/";
        // dm.baseURL = "file://E:/nginx-1.7.4/html/bundle/";
        //dm.baseURL = "file://E:/unity.projects/dungeon1/Assets/bundle/";
        //dm.baseURL = "file://E:/unity.projects/dungeon1/Assets/bundle/flash/";
        //dm.baseURL = "file://E:/unity.projects/dungeon1/Assets/bundle/android/";
        string url = App.config.bundleURL;// "http://192.168.1.78/bundle/";
        // string url = "http://192.168.2.105/bundle/";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                url = "jar:file://" + url;
                url += "android/";
                break;
            case RuntimePlatform.IPhonePlayer:
                url = "file:/" + url;
                url += "ios/";
                break;
            default:
                url = "file://" + url;
                url += "flash/";
                break;
        }

        dm.baseURL = url;

        Dictionary<string, int> versions = new Dictionary<string, int>();

        versions.Add("avatar/jianshi.u3d", 1);
        versions.Add("avatar/kulou.u3d", 1);
        versions.Add("avatar/pet_fire.u3d", 1);
        versions.Add("avatar/pet_thunder.u3d", 1);
        versions.Add("avatar/BOSS_wolf.u3d", 1);

        versions.Add("weapon/ld.u3d", 1);

        //effects
        versions.Add("effect/dg01.u3d", 1);
        versions.Add("effect/dg02.u3d", 1);
        versions.Add("effect/dg03.u3d", 1);
        versions.Add("effect/dg04.u3d", 1);
        versions.Add("effect/hurt.u3d", 1);
        versions.Add("effect/skill04.u3d", 1);
        versions.Add("effect/skill04_debuff.u3d", 1);
        versions.Add("effect/skill04_hurt.u3d", 1);


        versions.Add("scene/demoScene.u3d", 1);

        dm.versions = versions;

        List<Bundle> bundles = new List<Bundle>();

        //bundles.Add(new Bundle("scene/dungeon1.u3d"));
        bundles.Add(new Bundle("scene/demoScene.u3d"));

        bundles.Add(new Bundle("avatar/jianshi.u3d"));
        bundles.Add(new Bundle("avatar/kulou.u3d"));
        bundles.Add(new Bundle("avatar/pet_fire.u3d"));
        bundles.Add(new Bundle("avatar/pet_thunder.u3d"));
        bundles.Add(new Bundle("avatar/BOSS_wolf.u3d"));

        bundles.Add(new Bundle("weapon/ld.u3d"));
        //bundles.Add(new Bundle("weapon/bow.u3d"));
        // bundles.Add(new Bundle("weapon/sword.u3d"));

        //effects
        //bundles.Add(new Bundle("effect/arrow.u3d"));
        bundles.Add(new Bundle("effect/dg01.u3d"));
        bundles.Add(new Bundle("effect/dg02.u3d"));
        bundles.Add(new Bundle("effect/dg03.u3d"));
        bundles.Add(new Bundle("effect/dg04.u3d"));
        bundles.Add(new Bundle("effect/hurt.u3d"));
        bundles.Add(new Bundle("effect/skill04.u3d"));
        bundles.Add(new Bundle("effect/skill04_debuff.u3d"));
        bundles.Add(new Bundle("effect/skill04_hurt.u3d"));

        //monster attack effect
        // bundles.Add(new Bundle("effect/magic_fire_01.u3d"));
        dm.downlod(bundles, this);
    }
    
    private int index;
    private int currentIndex;
    List<string> prefabNames;
    public void checkUsePrefab()
    {
        prefabNames = new List<string>();
        Dictionary<int, SkillEffectTemplate> skilleffects = App.template.getTemps<SkillEffectTemplate>();
        foreach (SkillEffectTemplate st in skilleffects.Values){
            if (st.bulletPrefab != null) prefabNames.Add(Naming.ColliderPath + st.bulletPrefab);
            if (st.hitEffect != null) prefabNames.Add(Naming.EffectPath + st.hitEffect);
        }
        prefabNames.Add(Naming.EffectPath + "hurt");
        prefabNames.Add(Naming.EffectPath + "boss_atk");
        prefabNames.Add(Naming.EffectPath + "boss_skill");
        prefabNames.Add(Naming.EffectPath + "fireball");
        prefabNames.Add(Naming.EffectPath + "firepet_atk");
        prefabNames.Add(Naming.EffectPath + "firepet_ex");
        prefabNames.Add(Naming.EffectPath + "firepet_skill");
        prefabNames.Add(Naming.EffectPath + "firepet_skillstart");
        prefabNames.Add(Naming.EffectPath + "firepet_summon");
        prefabNames.Add(Naming.EffectPath + "montser_chong_atk");
        prefabNames.Add(Naming.EffectPath + "montser_magicball");
        prefabNames.Add(Naming.EffectPath + "skill04_debuff");
        prefabNames.Add(Naming.EffectPath + "skill04_new");
        prefabNames.Add(Naming.EffectPath + "thunder_atk");
        prefabNames.Add(Naming.EffectPath + "thunder_ex");
        prefabNames.Add(Naming.EffectPath + "thunder_skill");
        prefabNames.Add(Naming.EffectPath + "thunder_skillstart");
        prefabNames.Add(Naming.EffectPath + "thunder_summon");
        //avatar
        prefabNames.Add(Naming.AvatarPath + "BOSS_zhizhu");
        prefabNames.Add(Naming.AvatarPath + "chong");
        prefabNames.Add(Naming.AvatarPath + "gbl");
        prefabNames.Add(Naming.AvatarPath + "gbl_jin");
        prefabNames.Add(Naming.AvatarPath + "man");
        prefabNames.Add(Naming.AvatarPath + "pet_fire");
        prefabNames.Add(Naming.AvatarPath + "pet_thunder");

        currentIndex = 0;
    }
    public void load()
    {
        showtip(LoadingState.xml);
        App.template.loadxml(()=> { App.coroutine.StartCoroutine(loadPrefab()); });
    }


    public IEnumerator loadPrefab()
    {
        showtip(LoadingState.prefab);
        checkUsePrefab();
        yield return 2;
        while (currentIndex < prefabNames.Count) {
            App.res.loadPrefab(prefabNames[currentIndex++]);
            float progress = (float)currentIndex / prefabNames.Count;
            onProgress(progress, 1f);
            yield return 2;
        }
        //shaders
        string[] shaders = new string[] { "BeHit2", "Custom/Dissolve", "Particles / Additive" };
        foreach (string name in shaders) {
            Shader.Find(name);
        }
        prefabNames.Clear();
        yield return 2;
        RedScreen.instance.createRedTexture();
        yield return 2;
        App.coroutine.StartCoroutine(loadScene());
        App.coroutine.StartCoroutine(onSceneProgress());
    }

    IEnumerator loadScene() {
        showtip(LoadingState.scene);
        async = Application.LoadLevelAsync("map01");
        async.allowSceneActivation = false;
        yield return async;    
    }

    IEnumerator onSceneProgress() {
        while (async.progress < 0.9f) {
            yield return 1;
            onProgress(async.progress/0.9f, 1f);
        }
        async.allowSceneActivation = true;
    }
    

    public void finishload(){
        go.SetActive(false);
    }
}
