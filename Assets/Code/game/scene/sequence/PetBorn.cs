using System.Collections;
using System.Linq;
using engine;
using UnityEngine;

public class PetBorn {
    private GameObject blackObj;
    private GameObject newCamera;
    private Vector3 origRotate;
    private GameObject summonEffect;
    private ParticleSystem[] allParticles;
    private ParticleSystem specialParticle;
    private Animation[] anims;
    private Animator[] animators;
    private Pet pet;
    //private WallVisionOutlineEffect wallEffect;

    public void beginBorn(int index, PetData data) {
        pet = CharFactory.createPet(data, (PetPosition)index, Player.instance.agent.walkableMask);
        pet.transform.position = Player.instance.Position;
        pet.agent.walkableMask = Player.instance.agent.walkableMask;
        pet.uiIndex = index;
        BattleEngine.scene.addFriend(pet);
        pet.model.SetActive(false);
        pet.prepareSkill(data.summonSkill);
        Player.instance.animator.SetBool(Hash.summonBool, true);
        UIManager.Instance.Enable = false;
        Time.timeScale = CommonTemp.petScale;
        summonEffect = Engine.res.createObj("Local/prefab/effect/" + data.charTemplate.summonEffect, pet.Position);
        Transform trans = summonEffect.T(CommonTemp.eventNames[index]);
        if (trans != null) {
            Binding b = trans.gameObject.AddComponent<Binding>();
            b.data = pet;
        }
        Transform t = summonEffect.T("tishi");
        if (t == null) {
            Debug.LogError("summon effect not have tishi, error!!!!");
            return;
        }
        specialParticle = t.GetComponent<ParticleSystem>();
        anims = summonEffect.GetComponentsInChildren<Animation>();
        animators = summonEffect.GetComponentsInChildren<Animator>();
        allParticles = summonEffect.GetComponentsInChildren<ParticleSystem>();
        CameraManager.setLayerRecursively(summonEffect, summonEffect.layer = LayerMask.NameToLayer("Special"));
        CameraManager.setLayerRecursively(Player.instance.model, Player.instance.model.layer = LayerMask.NameToLayer("Special"));
        if (CommonTemp.rotates[index])
            summonEffect.transform.rotation = Player.instance.transform.rotation;
        createNewCam();
        //wallEffect = CameraManager.Main.GetComponent<WallVisionOutlineEffect>();
        //wallEffect.enabled = false;
        App.coroutine.StartCoroutine(blackSceen(CommonTemp.blackTimes[index]));
        App.coroutine.StartCoroutine(playEffect(specialParticle.startDelay, specialParticle.startLifetime));
        App.coroutine.StartCoroutine(beginResolve(CommonTemp.bornTimes[index], pet));
        foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
            c.HpBar.setVisible(false, true);
        }
        foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
            c.HpBar.setVisible(false, true);
        }
        //App.coroutine.StartCoroutine(playSkill(CommonTemp.hurtTimes[index],CommonTemp.skillIDs[index]));
    }

    private void createNewCam() {
        newCamera = new GameObject();
        newCamera.transform.position = CameraManager.Main.transform.position;
        newCamera.transform.rotation = CameraManager.Main.transform.rotation;
        newCamera.transform.localScale = CameraManager.Main.transform.localScale;
        newCamera.name = "newCamera";
        Camera m = newCamera.AddComponent<Camera>();
        m.fieldOfView = CameraManager.Main.fieldOfView;
        m.nearClipPlane = CameraManager.Main.nearClipPlane;
        m.farClipPlane = CameraManager.Main.farClipPlane;
        m.clearFlags = CameraClearFlags.Depth;
        m.depth = 10;
        m.cullingMask = 1 << LayerMask.NameToLayer("Special");
    }


    private IEnumerator blackSceen(float time) {
        yield return new WaitForSeconds(time);
        BattleEngine.scene.pause(true);
        blackObj = new GameObject("black");
        float alpha = CommonTemp.blackAlpha;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            alpha = CommonTemp.blackAlphaIOS;
        blackObj.transform.position = new Vector3(.5f, .5f, 1000);
        blackObj.AddComponent<GUITexture>();
        blackObj.guiTexture.texture = iTween.CameraTexture(Color.black);
        blackObj.guiTexture.color = new Color(.5f, .5f, .5f, alpha);
    }

    private IEnumerator playEffect(float time, float pauseTime) {
        yield return new WaitForSeconds(time);
        //foreach (ParticleSystem s in allParticles) {
        //    if (s != specialParticle) {
        //        s.Pause();
        //    }
        //}
        //foreach (Animation a in anims) {
        //    a.Stop();
        //}
        //foreach (Animator a in animators) {
        //    a.enabled = false;
        //}
        CameraManager.shakeCamera(CameraManager.Main, 0.5f, 2);
        App.coroutine.StartCoroutine(removePause(pauseTime));
    }

    public IEnumerator removePause(float time) {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;
        //foreach (ParticleSystem s in allParticles) {
        //    if (s != specialParticle) {
        //        s.Play();
        //    }
        //}
        //foreach (Animation a in anims) {
        //    a.Play();
        //}
        //foreach (Animator a in animators) {
        //    a.enabled = true;
        //}
        if (blackObj)
            Object.Destroy(blackObj);
        if (summonEffect != null)
            CameraManager.setLayerRecursively(summonEffect, summonEffect.layer = LayerMask.NameToLayer("Avatars"));
        CameraManager.setLayerRecursively(Player.instance.model, Player.instance.model.layer = LayerMask.NameToLayer("Avatars"));
        UIManager.Instance.Enable = true;
        BattleEngine.scene.pause(false);
        Object.Destroy(newCamera);
        foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
            c.HpBar.setVisible(true);
        }
        foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
            c.HpBar.setVisible(true);
        }
        Player.instance.animator.SetBool(Hash.summonBool, false);
    }

    private IEnumerator beginResolve(float time, Pet pet) {
        yield return new WaitForSeconds(time);
        pet.model.SetActive(true);
        pet.playSummon();
        pet.startReverseDissolve();
        if (summonEffect)
            Object.Destroy(summonEffect, 3f);
        yield return new WaitForSeconds(3f);
        //wallEffect.enabled = true;
    }
}
