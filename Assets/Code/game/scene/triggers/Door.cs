using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using engine;
public class Door :SceneTrigger {
    //public static void initPlayerDoorState(string doorName, bool open) {
    //    int mask = 1 << NavMesh.GetNavMeshLayerFromName(doorName);
    //    if (open) {
    //        Player.instance.agent.walkableMask |= mask;
    //    } else {
    //        Player.instance.agent.walkableMask &= ~mask;
    //    }
    //}
   



    public string state;//default state
    private EnterRoomTrigger enterRoomTrigger;
    private Dictionary<FightCharacter, int> triggered=new Dictionary<FightCharacter,int>();
    public override void reset(GameObject go,DungeonTemplate.DungeonTrigger trigger) {
        base.reset(go, trigger);
        state = trigger.state;

        triggered.Clear();
    }
    public void setEnterRoomTriggerPrefab(GameObject enterRoomTriggerObject) {
        //enter room trigger
        if (enterRoomTriggerObject != null) {
            enterRoomTrigger = new EnterRoomTrigger();
            enterRoomTrigger.door = this;
            enterRoomTrigger.reset(enterRoomTriggerObject, trigger);
        }

    }
   
    public override void onTrigger() {
        UIManager.Instance.Enable = false;
        CameraManager.CameraFollow.enabled = false;
        beginRotate();
    }
    
    public void onEnterRoomTrigger(FightCharacter c, EnterRoomTrigger t) {
        if (!triggered.ContainsKey(c) && (c.isHero() || c==Player.instance))
        {
            triggered[c] = 1;
            setOpen(c, false);
        }

        if (!t.triggered)
         {
             if ((!Player.instance.isDead() && c == Player.instance) || Player.instance.isDead())
             {
                 t.triggered = true;
                 App.sceneManager.spawnMonsters();
             }
        }
        if (triggered.Count >= BattleEngine.scene.getFriends().Count)
        {
            t.onTrigger();
            close();
        }
    }

    private void setOpen(FightCharacter c, bool open) {
        int mask = 1 << NavMesh.GetNavMeshLayerFromName(name);

        if (open) {
            c.agent.walkableMask |= mask;
        } else {
            c.agent.walkableMask &= ~mask;
        }
    }
    public void initFriendsDoorState() {
        if (state == "open") {
            setFriendsDoorOpen(true);
            model.SetActive(false);
        } else {
            setFriendsDoorOpen(false);
        }
    }
    private void setFriendsDoorOpen(bool open) {
        int mask = 1 << NavMesh.GetNavMeshLayerFromName(this.name);
        if (open) {
            Player.instance.agent.walkableMask |= mask;
        } else {
            Player.instance.agent.walkableMask &= ~mask;
        }
        mask = Player.instance.agent.walkableMask;
        foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
            c.agent.walkableMask = mask;
        }
    }
   

    public void close() {
        //setFriendsDoorOpen(false);
        model.SetActive(true);
        DoorAnim[] anims = model.GetComponentsInChildren<DoorAnim>();
        if (anims != null && anims.Length > 0) {
            foreach (DoorAnim anim in anims) {
                anim.beginPlay();
            }
        }
    }

    private void beginRotate() {
        BattleEngine.scene.pauseAI(true);
        GameObject callBackObj = createCallBack(model, trigger);
        Vector3 rotate = callBackObj.transform.localEulerAngles;
        Hashtable ht1 = iTween.Hash("rotation", rotate, "easeType", iTween.EaseType.linear, "oncomplete", "beginPlay", "time", CommonTemp.toDoorTime,
            "oncompletetarget", callBackObj);
        iTween.RotateTo(Camera.main.gameObject, ht1);
        Hashtable ht2 = iTween.Hash("position", callBackObj.transform.position, "easeType", iTween.EaseType.linear, "time", CommonTemp.toDoorTime);
        iTween.MoveTo(Camera.main.gameObject, ht2);
       // obstacle.enabled = false;
    }

    private GameObject createCallBack(GameObject model, DungeonTemplate.DungeonTrigger trigger) {
        GameObject obj = new GameObject();
        Vector3 pos = model.transform.position;
        Vector3 offset = Player.instance.Position - model.transform.position;
        pos += offset.normalized * CommonTemp.doorOffset;
        pos.y = model.transform.position.y + CommonTemp.doorCameraY;
        obj.transform.position = pos;
        obj.transform.localRotation = Camera.main.transform.localRotation;
        obj.transform.localScale = Camera.main.transform.localScale;
        obj.transform.LookAt(model.transform);
        TweenDoor tween = obj.AddComponent<TweenDoor>();
        tween.door = this;
        tween.model = model;
        tween.trigger = trigger;
        tween.origRotate = Camera.main.transform.localEulerAngles;
        tween.origPosition = Camera.main.transform.position;
        return obj;
    }

    public class TweenDoor:MonoBehaviour {
        public Door door;
        public GameObject model;
        public DungeonTemplate.DungeonTrigger trigger;
        public Vector3 origRotate;
        public Vector3 origPosition;

        private float effectTime;

        void beginPlay() {
            //model.SetActive(false);
            //if (trigger.animation != null) {
            //    Animation anim = model.GetComponent<Animation>();
            //    if (anim != null) {
            //        anim.Play();
            //    }
            //}
            //if (trigger.effect != null) {
            //    GameObject effect = Engine.res.createSingle(trigger.effect);
            //    effect.transform.position = model.transform.position;
            //    ParticleSystem p = effect.GetComponent<ParticleSystem>();
            //    effectTime = p.startLifetime;
            //    StartCoroutine(endPlay());
            //    Destroy(effect, 1f);
            //}
            effectTime = 0;
            DoorAnim[] anims = model.GetComponentsInChildren<DoorAnim>();
            if (anims != null && anims.Length > 0) {
                foreach (DoorAnim anim in anims) {
                    anim.stopPlay();
                    //anim.callBack = finishPlay;
                    ParticleSystem p = anim.GetComponent<ParticleSystem>();
                    if (p != null && p.startLifetime > effectTime)
                        effectTime = p.startLifetime;
                }
            }
            StartCoroutine(endPlay());
        }

        IEnumerator endPlay() {
            yield return new WaitForSeconds(effectTime);
            Hashtable ht1 = iTween.Hash("rotation", origRotate, "easeType", iTween.EaseType.linear, "oncomplete", "recover", "time", CommonTemp.fromDoorTime,
                "oncompletetarget", gameObject);
            iTween.RotateTo(Camera.main.gameObject, ht1);
            Hashtable ht2 = iTween.Hash("position", origPosition, "easeType", iTween.EaseType.linear, "time", CommonTemp.fromDoorTime);
            iTween.MoveTo(Camera.main.gameObject, ht2);
        }

        //void finishPlay() {
        //    Hashtable ht1 = iTween.Hash("rotation", origRotate, "easeType", iTween.EaseType.linear, "oncomplete", "recover", "time", 1f,
        //       "oncompletetarget", gameObject);
        //    iTween.RotateTo(Camera.main.gameObject, ht1);
        //    Hashtable ht2 = iTween.Hash("position", origPosition, "easeType", iTween.EaseType.linear, "time", 1f);
        //    iTween.MoveTo(Camera.main.gameObject, ht2);
        //}

        void recover() {
            recoverCamera();
            App.sceneManager.onDoorOpened();
            App.input.setJoyEnabled(true);

            door.setFriendsDoorOpen(true);
            
        }

        void recoverCamera() {
            UIManager.Instance.Enable = true;
            CameraManager.CameraFollow.enabled = true;
            Destroy(gameObject);
            model.SetActive(false);
            BattleEngine.scene.pauseAI(false);
            BattleEngine.scene.dungeonData.currentTriggerIndex++;
            Player.instance.calcNextPos = true;
            BattleEngine.scene.dungeonData.clearCount = BattleEngine.scene.getFriends().Count;
            //Destroy(model);
        }
    }
}
