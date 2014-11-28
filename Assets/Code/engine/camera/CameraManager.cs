using System.Collections;
using System.Linq;
using engine;
using UnityEngine;

public class CameraManager {
    private static GameObject blackObj;
    private static GameObject newCamera;
    private static Vector3 origRotate;
    private static GameObject summonEffect;
    private static float origView = -1;
    private static bool hasShake = false;
    private static Camera mainCamera;

    public static void focusObj(GameObject focus, GameObject effect) {
        if (newCamera != null)
            return;
        BattleEngine.scene.pause(true);
        UIManager.Instance.Enable = false;
        Time.timeScale = CommonTemp.petScale;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            blackSceen(CommonTemp.blackAlphaIOS);
        else
            blackSceen(CommonTemp.blackAlpha);
        setLayerRecursively(focus, focus.layer = LayerMask.NameToLayer("Special"));
        setLayerRecursively(effect, effect.layer = LayerMask.NameToLayer("Special"));
        summonEffect = effect;
        createNewCam(focus);       
    }

    public static void removeFocus(GameObject focus) {
        //Time.timeScale = 1f;
        //if(blackObj)
        //    Object.Destroy(blackObj, CommonTemp.tweenNormalTime);
        //setLayerRecursively(focus, focus.layer = LayerMask.NameToLayer("Default"));
        //if(summonEffect != null)
        //    setLayerRecursively(summonEffect, summonEffect.layer = LayerMask.NameToLayer("Default"));
        //iTween.RotateTo(Main.gameObject, origRotate, CommonTemp.tweenNormalTime);
        //App.coroutine.StartCoroutine(addCameraFollow());
        //Hashtable ht = iTween.Hash("from", newCamera.camera.fieldOfView, "to", origView, "time", CommonTemp.tweenNormalTime, "onupdate", "changeField",
        //    "oncomplete", "destroyObj");
        //iTween.ValueTo(newCamera, ht);
    }

    public static void shakeCamera(Camera camera, float time) {
        if(hasShake)
            return;;
        hasShake = true;     
        int num = (int)(time / BattleConfig.everyTime);
        if (origView < 0)
            origView = Main.fieldOfView;
        App.coroutine.StartCoroutine(beginShake(camera,num));
    }

    private static void createNewCam(GameObject focus) {
        newCamera = new GameObject();
        newCamera.transform.position = Main.transform.position;
        newCamera.transform.rotation = Main.transform.rotation;
        newCamera.transform.localScale = Main.transform.localScale;
        newCamera.name = "newCamera";
        newCamera.AddComponent<ChangeField>();
        Camera m = newCamera.AddComponent<Camera>();
        m.fieldOfView = Main.fieldOfView;
        m.nearClipPlane = Main.nearClipPlane;
        m.nearClipPlane = Main.farClipPlane;
        m.clearFlags = CameraClearFlags.Depth;
        m.depth = 10;
        m.cullingMask = 1 << LayerMask.NameToLayer("Special");
        newCamera.transform.LookAt(focus.transform);
        Vector3 finalRotate = newCamera.transform.localEulerAngles;
        newCamera.transform.localEulerAngles = Main.transform.localEulerAngles;
        origRotate = Main.transform.localEulerAngles;
        CameraFollow.enabled = false;
        Hashtable ht = iTween.Hash("rotation", finalRotate, "easeType", iTween.EaseType.linear, "time", CommonTemp.tweenPetTime);
        iTween.RotateTo(Main.gameObject, ht);
        iTween.RotateTo(newCamera.gameObject, ht);
        if(origView < 0)
            origView = Main.fieldOfView;
        Hashtable ht1 = iTween.Hash("from", origView, "to", Main.fieldOfView - CommonTemp.petView, "time", CommonTemp.tweenPetTime, "onupdate", "changeField");
        iTween.ValueTo(newCamera, ht1);
    }

    private static void blackSceen(float alpha = 0.3f) {
        blackObj = new GameObject("black");
        blackObj.transform.position = new Vector3(.5f, .5f, 1000);
        blackObj.AddComponent<GUITexture>();
        blackObj.guiTexture.texture = iTween.CameraTexture(Color.black);
        blackObj.guiTexture.color = new Color(.5f, .5f, .5f, alpha);
    }

    public static void setLayerRecursively(GameObject go, int layerNumber) {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
            trans.gameObject.layer = layerNumber;
        }
        go.layer = layerNumber;
    }

    private static IEnumerator addCameraFollow() {
        yield return new WaitForSeconds(CommonTemp.tweenNormalTime);
        CameraFollow.enabled = true;
    }

    private static IEnumerator beginShake( Camera cam, int num) {
        for (int i = 0; i < num; i++) {
            yield return new WaitForSeconds(BattleConfig.everyTime);
            float range = 0;
            if (i == num - 1) {
                range = 0;
                hasShake = false;
            }
            else if (i % 2 == 0)
                range = -BattleConfig.scope;
            else
                range = BattleConfig.scope;
            cam.fieldOfView = origView + range;
        }    
    }

    public static Camera Main {
       get {
           return mainCamera; 
       }

        set
        {
            mainCamera = value;
        }
    }

    public static CameraFollowing CameraFollow {
        get {
            return Main.GetComponent<CameraFollowing>();
        }
    }

    class ChangeField:MonoBehaviour {
        
        void changeField(float value) {
            camera.fieldOfView = value;
            Main.fieldOfView = value;
        }

        void destroyObj() {
            UIManager.Instance.Enable = true;
            BattleEngine.scene.pause(false);
            Destroy(gameObject);
        }
    }
}
