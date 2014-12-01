using UnityEngine;
using System.Collections;
using engine;

public class SceneManager  {
    DungeonScene current;
    public void clear()
    {
        if (current != null) current.clear();
        BattleUI.instance.clear();
        //App.res.clear();
        current = null;
        BattleEngine.scene = null;
        BoomBoxManager.instance.clear();
        DropManager.instance.clear();
        ArrawManager.instance.clear();
    }
    public void onSceneLoaded(string sceneName) {
        App.input.init(true);
        BattleEngine.reset();
        
        if(BattleEngine.scene==null)BattleEngine.scene = new DungeonScene();
        DungeonData data=createDungeonData(1);
        BattleEngine.scene.reset(data);
        
        current = BattleEngine.scene as DungeonScene;
        BattleUI.instance.reset();
        firstEnterDungeon = true;
    }
    private static int dungeonInstanceId;
    private DungeonData createDungeonData(int id)
    {
        DungeonTemplate template = App.template.getTemp<DungeonTemplate>(id);
        DungeonData data = new DungeonData(++dungeonInstanceId, id);
        //foreach (DataPoint point in template.dataPoints) {
        //    MonsterInfo info = new MonsterInfo(++monsterInstanceId, point.templateId);
        //    data.monsters.Add(info);
        //}
        return data;
    }

    public void onEnterSequenceEnded() {
        //monster spawn is triggered by colliding with "MonsterTrigger" in the scene.
    }

    public void onSceneTrigger(FightCharacter c, SceneTrigger trigger) {
        if (current == null) return;
        if (trigger is Door) {
            tryEnterDoor(trigger as Door);
        } else if (trigger is MonsterSpawnTrigger) {
            trigger.onTrigger();
            spawnMonsters();
        } else if (trigger is EnterRoomTrigger) {
            EnterRoomTrigger enterRoomTrigger = trigger as EnterRoomTrigger;
            enterRoomTrigger.door.onEnterRoomTrigger(c,enterRoomTrigger);
        }
    }

    public void tryEnterDoor(Door door) {
        if (current.isRoomCleared() && current.hasNextRoom()) {
            door.onTrigger();
        }
       
    }
    private bool firstEnterDungeon = true;
    public void spawnMonsters() {
        if (firstEnterDungeon) {
            ScriptManager.instance.onComplete = doSpawnMonsters;
            ScriptManager.instance.trigger(1);
        } else {
            doSpawnMonsters();  
        }
    }
    private void doSpawnMonsters() {
        if (firstEnterDungeon) {
            firstEnterDungeon = false;
            current.prepareRoom();
           // current.spawnSpecialMonster(10003);
        } else {
            current.nextRoom();
            ArrawManager.instance.hideCopyArraw();
            if (current.dungeonData.currentRoomIndex == 1) {
                current.spawnSpecialMonster(10003);
            }
        }
        current.checkMonsterGroup = true;
    }
    public void changeBgSound(string sound)
    {
        AudioClip clip = Engine.res.loadSound("Local/sound/" + sound);
        if(clip!=null){
            App.audioSource.clip = clip;
            App.audioSource.Play();
        }
    }
    public void onDoorOpened() {
        Vector3 v = current.getNextRoomPosition();
        if (v == Vector3.zero) return;
        ArrawManager.instance.showCopyArraw(v);
    }
}
