using UnityEngine;
using System.Collections;
using engine;
public class App  {
    //managers for easy access
    public static BundleManager bundle;
    public static MonoBehaviour coroutine;
    public static InputManager input;
    public static bool suspend = false;
    public static AudioSource audioSource;

    public static IResourceManager res;
    public static ISoundManager sound;
    public static Templates template;
    public static IAttackController battleController;

    public static LocalResourceManager local;

    public static SceneManager sceneManager;

    public static IColliderManager collider;

    public static DefaultEffectManager effect;


    public static AnimationManager animEventManager;


    public static Configuration config;

    public static void init(GameObject bootObject) {


        config = bootObject.addOnce<Configuration>();

        input = new InputManager(bootObject.getChild("inputManager"));

        App.bundle = bootObject.AddComponent<BundleManager>();
        App.coroutine = bootObject.AddComponent<CoroutineManager>();
       
       
        App.local = new LocalResourceManager();
        App.res = Engine.res = new ResourceManager(bundle);
        App.sound = Engine.sound = new SoundManager();
        App.audioSource = bootObject.getChildComponent<AudioSource>("Sound");
        BattleEngine.effect = App.effect = new DefaultEffectManager();
        App.sceneManager = new SceneManager();


        template = BattleEngine.template = new Templates();
        battleController = BattleEngine.controller = new BattleController();

        collider = BattleEngine.collider = new ColliderManager();

        Engine.animEventManager = animEventManager = new AnimationManager();

        //BattleEngine.aiEnv = new AIEnv();
        BattleEngine.petEnv = new PetAIEnv();
        BattleEngine.monsterEnv = new MonsterAIEnv();

        (res as DefaultResourceManager).init();

        template.init();

        BattleConfig.init();

    }
   
    public static void update() {
        if(suspend)
            return;
        input.update();
        if (BattleEngine.scene != null) {
            BattleEngine.scene.update();
            BattleUI.instance.update();
        }
        effect.update();
    }

    //public static void initInput(bool enableJoystick=false) {
    //    input.SetActive(true);

    //    GameObject joystick = input.getChild("joystick");
    //    joystick.SetActive(enableJoystick);

    //}
   
}
