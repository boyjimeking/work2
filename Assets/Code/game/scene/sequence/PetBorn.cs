using System.Collections;
using System.Linq;
using engine;
using UnityEngine;

public class PetBorn {
    private GameObject blackObj;
    private GameObject newCamera;
    private Vector3 origRotate;
    private GameObject summonEffect;

    public void beginBorn(int index, PetData data) {
        Pet pet = CharFactory.createPet(data, (PetPosition)index, Player.instance.agent.walkableMask);
        pet.transform.position = Player.instance.Position;
        pet.agent.walkableMask = Player.instance.agent.walkableMask;
        pet.uiIndex = index;
        BattleEngine.scene.addFriend(pet);
        pet.playSummon();
        BattleEngine.scene.pause(true);
        UIManager.Instance.Enable = false;
        Time.timeScale = CommonTemp.petScale;
        summonEffect = Engine.res.createObj("Local/prefab/effect/" + data.charTemplate.summonEffect, pet.Position);
        Transform trans = summonEffect.T(CommonTemp.eventNames[index]);
        if (trans != null) {
            Binding b = trans.gameObject.AddComponent<Binding>();
            b.data = pet;
        }
        CameraManager.setLayerRecursively(summonEffect, summonEffect.layer = LayerMask.NameToLayer("Special"));
        CameraManager.setLayerRecursively(Player.instance.model, Player.instance.model.layer = LayerMask.NameToLayer("Special"));
        if (CommonTemp.rotates[index])
            summonEffect.transform.rotation = Player.instance.transform.rotation;
        createNewCam();
        App.coroutine.StartCoroutine(blackSceen(CommonTemp.blackTimes[index]));
        App.coroutine.StartCoroutine(playEffect(CommonTemp.effectTimes[index], CommonTemp.effectPrefabs[index]));
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
        blackObj = new GameObject("black");
        float alpha = CommonTemp.blackAlpha;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            alpha = CommonTemp.blackAlphaIOS;
        blackObj.transform.position = new Vector3(.5f, .5f, 1000);
        blackObj.AddComponent<GUITexture>();
        blackObj.guiTexture.texture = iTween.CameraTexture(Color.black);
        blackObj.guiTexture.color = new Color(.5f, .5f, .5f, alpha);
    }

    private IEnumerator playEffect(float time, string path) {
        yield return new WaitForSeconds(time); 
        if (!string.IsNullOrEmpty(path)) {
            GameObject effect = App.res.createSingle(path);
            effect.transform.position = summonEffect.transform.position;
            if (effect != null) {}
        }
        App.coroutine.StartCoroutine(removePause(0.2f));
    }

    public IEnumerator removePause(float time) {
        yield return new WaitForSeconds(time); 
        Time.timeScale = 1f;
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
    }

    private IEnumerator beginResolve(float time, Pet pet) {
        yield return new WaitForSeconds(time);
        pet.startReverseDissolve();
        if (summonEffect)
            Object.Destroy(summonEffect, 3f);
    }
}
