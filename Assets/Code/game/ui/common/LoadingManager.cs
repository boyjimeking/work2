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
        //effect
         prefabNames.Add(Naming.EffectPath + "boss_atk");
         prefabNames.Add(Naming.EffectPath + "boss_hurt");
         prefabNames.Add(Naming.EffectPath + "boss_skill");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_atk");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_skill01");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_skill01start");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_skill02");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_skill02atk");
         prefabNames.Add(Naming.EffectPath + "BOSS_spider_skill02start");
         prefabNames.Add(Naming.EffectPath + "bug_appear");
         prefabNames.Add(Naming.EffectPath + "dg01");
         prefabNames.Add(Naming.EffectPath + "dg02");
         prefabNames.Add(Naming.EffectPath + "dg03");
         prefabNames.Add(Naming.EffectPath + "dg04");
         prefabNames.Add(Naming.EffectPath + "door");
         prefabNames.Add(Naming.EffectPath + "fireball");
         prefabNames.Add(Naming.EffectPath + "firehurt");
         prefabNames.Add(Naming.EffectPath + "firepet_atk");
         prefabNames.Add(Naming.EffectPath + "firepet_ex");
         prefabNames.Add(Naming.EffectPath + "firepet_skillstart");
         prefabNames.Add(Naming.EffectPath + "fireskill_end");
         prefabNames.Add(Naming.EffectPath + "fireskill_start");
         prefabNames.Add(Naming.EffectPath + "fire_summon");
         prefabNames.Add(Naming.EffectPath + "goblin_appear");
         prefabNames.Add(Naming.EffectPath + "hurt");
         prefabNames.Add(Naming.EffectPath + "hurt_ground");
         prefabNames.Add(Naming.EffectPath + "monster_appear");
         prefabNames.Add(Naming.EffectPath + "monster_chong_atk");
         prefabNames.Add(Naming.EffectPath + "monster_chong_hurt");
         prefabNames.Add(Naming.EffectPath + "monster_disappear");
         prefabNames.Add(Naming.EffectPath + "monster_magicball");
         prefabNames.Add(Naming.EffectPath + "monster_magic_hurt");
         prefabNames.Add(Naming.EffectPath + "move");
         prefabNames.Add(Naming.EffectPath + "skill04_debuff");
         prefabNames.Add(Naming.EffectPath + "skill04_hurt");
         prefabNames.Add(Naming.EffectPath + "skill04_new");
         prefabNames.Add(Naming.EffectPath + "stop");
         prefabNames.Add(Naming.EffectPath + "stop_end");
         prefabNames.Add(Naming.EffectPath + "s_lightning");
         prefabNames.Add(Naming.EffectPath + "thunderpet_skill");
         prefabNames.Add(Naming.EffectPath + "thunderpet_skillstart");
         prefabNames.Add(Naming.EffectPath + "thunder_atk");
         prefabNames.Add(Naming.EffectPath + "thunder_hurt");
         prefabNames.Add(Naming.EffectPath + "thunder_summon");
         prefabNames.Add(Naming.EffectPath + "xuanyun");

        //collider
         prefabNames.Add(Naming.ColliderPath + "atk3");
         prefabNames.Add(Naming.ColliderPath + "chong_atk");
         prefabNames.Add(Naming.ColliderPath + "gbl_atk");
         prefabNames.Add(Naming.ColliderPath + "huo_atk1");
         prefabNames.Add(Naming.ColliderPath + "huo_skill1");
         prefabNames.Add(Naming.ColliderPath + "huo_sommon");
         prefabNames.Add(Naming.ColliderPath + "kulou");
         prefabNames.Add(Naming.ColliderPath + "lang_atk1");
         prefabNames.Add(Naming.ColliderPath + "lang_skill1");
         prefabNames.Add(Naming.ColliderPath + "lei_atk1");
         prefabNames.Add(Naming.ColliderPath + "lei_skill1");
         prefabNames.Add(Naming.ColliderPath + "man_skill1_dam");
         prefabNames.Add(Naming.ColliderPath + "man_skill1_la");
         prefabNames.Add(Naming.ColliderPath + "spider_atk");
         prefabNames.Add(Naming.ColliderPath + "spider_skill1");
         prefabNames.Add(Naming.ColliderPath + "spider_skill2");

       

        //avatar
        prefabNames.Add(Naming.AvatarPath + "BOSS_zhizhu");
        prefabNames.Add(Naming.AvatarPath + "chong");
        prefabNames.Add(Naming.AvatarPath + "gbl");
        prefabNames.Add(Naming.AvatarPath + "man");
        prefabNames.Add(Naming.AvatarPath + "pet_fire");
        prefabNames.Add(Naming.AvatarPath + "pet_thunder");


        //other prefab
        prefabNames.Add("Local/sequence/bornSEQ");
        prefabNames.Add("Local/UI/bossUI/bossBorn");
        prefabNames.Add("Local/UI/bossUI/bossBorn");
        prefabNames.Add("Local/UI/HP_UI/head_board");
        prefabNames.Add("Local/sequence/scriptClip/script11/Sequence");
        prefabNames.Add("Local/prefab/arraw/monsterArraw");
        prefabNames.Add("Local/prefab/floatingtext/FloatTextPlayer");
        prefabNames.Add("Local/prefab/floatingtext/EffectNormal");
        prefabNames.Add("Local/prefab/floatingtext/EffectCritical");
        prefabNames.Add("Local/prefab/floatingtext/EffectEva");
        prefabNames.Add("Local/prefab/floatingtext/FloatTextEnemy");
        prefabNames.Add("Local/prefab/mogu/Coin");
        prefabNames.Add("Local/prefab/arraw/Point");

        //sound
        App.res.loadSound("Local/sound/gblstar");
        App.res.loadSound("Local/sound/skill_10002");
        App.res.loadSound("Local/sound/hurt");
        App.res.loadSound("Local/sound/guaiwupg");
        App.res.loadSound("Local/sound/attack1");
        App.res.loadSound("Local/sound/attack2");
        App.res.loadSound("Local/sound/attack3");
        App.res.loadSound("Local/sound/skill_1000");
        App.res.loadSound("Local/sound/skill_1001");
        App.res.loadSound("Local/sound/skill_1011");
        App.res.loadSound("Local/sound/BOSS_bg");
        App.res.loadSound("Local/sound/bosspg");

        //controller
        App.res.loadObject("Local/controller/chongController");
        App.res.loadObject("Local/controller/gblController");
        App.res.loadObject("Local/controller/petFire");
        App.res.loadObject("Local/controller/petThunder");
        App.res.loadObject("Local/controller/BossSpiderController");
        App.res.loadObject("Local/material/pet_fire");
        App.res.loadObject("Local/material/pet_thunder");
        App.res.loadObject("Local/material/pet_fire");

        //texture
        Resources.Load("Local/picture/dissolve");
        

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
        Vector3 initPos = new Vector3(0f, -1000f, 0f);
        yield return 2;
        while (currentIndex < prefabNames.Count) {
            App.res.loadPrefab(prefabNames[currentIndex++]);
            //GameObject prefab = App.res.createObj(prefabNames[currentIndex++], initPos);
            //Object.Destroy(prefab, 0.2f);
            float progress = (float)currentIndex / prefabNames.Count;         
            onProgress(progress, 1f);
            yield return 2;
        }
        ////shaders
        //string[] shaders = new string[] {
        //    "BeHit2", "Custom/Dissolve", "BehindWall2","Particles/Additive" ,"Particles/Alpha Blended", "Particles/Alpha Blended 100",
        //    "Particles/Alpha Blended_sky", "Particles/Alpha Blended_sky", "effect/distortadd", "Transparent/Diffuse","Transparent/Specular",
        //    "echoLogin/Light/10-Fastest","echoLogin/Light/10-Fastest", "Transparent/Cutout/Diffuse","Xffect/heat_distortion","Mobile/Particles/Alpha Blened",
        //    "RimLightSpce", "echoLogin/Additive/FX/RimLit-1Color","echoLogin/Additive/FX/RimLit-Tex","echoLogin/Additive/21-Color"
        //};
        //foreach (string name in shaders) {
        //    Shader.Find(name);
        //}
        Shader.WarmupAllShaders();
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
        GameObject obj = new GameObject();
        obj.AddComponent<ExploderObject>();
        Object.Destroy(obj, 1f);
    }
}
