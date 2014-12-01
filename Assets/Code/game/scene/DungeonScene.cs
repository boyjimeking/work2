using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using engine;
public class DungeonScene : BattleScene {
    //public GameObject ground;
   // private GameObject born;

    private List<Door> doorList = new List<Door>();
    private GameObject sceneDecoration;
    private Monster boss;

    public override void clear()
    {
        base.clear();

        if (sceneDecoration != null)
        {
            GameObject.DestroyImmediate(sceneDecoration);
        }

    }
    protected void setupTriggers() {
        //doors
        doorList.Clear();
        DungeonTemplate.DungeonRoom[] rooms = dungeonData.template.rooms;
        if (rooms == null) return;
       GameObject enterRoomObject= sceneDecoration.getChild("enterRoom");
        for (int i = 0; i < rooms.Length; i++) {
            DungeonTemplate.DungeonTrigger trigger=rooms[i].doorTrigger;
            if(trigger!=null&&!string.IsNullOrEmpty(trigger.name)){
                 Door door = new Door();
                 door.reset(sceneDecoration.getChild(trigger.name), trigger);
                 door.setEnterRoomTriggerPrefab(enterRoomObject.getChild(trigger.enterRoomTriggerName));
                door.room = rooms[i];
                doorList.Add(door);
            }
        }

        //monster spawn triggers
        GameObject monsterTriggerRoot = sceneDecoration.getChild("MonsterTrigger");
        for (int i = 0; i < rooms.Length; i++) {
            DungeonTemplate.DungeonTrigger trigger = rooms[i].monsterSpawnTrigger;
            if (trigger != null) {
                MonsterSpawnTrigger t = new MonsterSpawnTrigger();
                t.reset(monsterTriggerRoot.getChild(trigger.name), trigger);
            }
        }
        BoomBoxManager.instance.init(dungeonData.template, sceneDecoration);
    }

    public override Vector3 getTriggerPos() {
        DungeonTemplate.DungeonTrigger trigger = dungeonData.template.triggers[dungeonData.currentTriggerIndex];
        if (trigger.isDoorTrigger()) {
            GameObject root = sceneDecoration.getChild("enterRoom");
            GameObject obj = root.getChild(trigger.enterRoomTriggerName);
            return obj.transform.position;
        }
        else {
                GameObject root = sceneDecoration.getChild("MonsterTrigger");
                GameObject obj = root.getChild(trigger.name);
            return obj.transform.position;
        }                     
    }

    public override void update()
    {
        //make sure the play and pets are in the same navmesh walkable layer
        //if (Door.lastOpenedDoor != null) {
        //    if(Player.instance.agent.enabled){
        //        int mask = Player.instance.agent.walkableMask;
        //        foreach (FightCharacter c in friends) {
        //            if (c != Player.instance && c.agent.enabled) {
                        
        //            }
        //        }
        //    }
           
        //}

        if (BattleEngine.petEnv != null) BattleEngine.petEnv.update();
        if (BattleEngine.monsterEnv != null) BattleEngine.monsterEnv.update();

        base.update();
        DropManager.instance.update();

        //check if player and all summoned pets are dead
        if (!BattleEngine.battleEnded) {
            bool allDead = true;
            foreach (FightCharacter c in friends) {
                if (!c.isDead()) {
                    allDead = false;
                    break;
                }
            }
            BattleEngine.battleEnded = allDead;
        }
    }

    public override void reset(DungeonData data) {
        base.reset(data);
        specialMonsters.Clear();
        //loading scene trigger prefab
        GameObject prefab = Engine.res.loadPrefab(data.template.triggerPrefab);
        if (prefab != null) {
            sceneDecoration = GameObject.Instantiate(prefab) as GameObject;
        }
        setupTriggers();

        createContent();

        App.sceneManager.changeBgSound(data.template.sound);
    }
    private void createContent() {
        if (App.config.USE_BUNDLE) {
            App.bundle.bulkLoad(new string[] { 
           //avatar
           "avatar/jianshi.u3d",
           "avatar/kulou.u3d",
           "avatar/pet_fire.u3d",
           "avatar/pet_thunder.u3d",
           "avatar/BOSS_wolf.u3d",
           //weapon
           "weapon/ld.u3d",
           //effects
           "effect/dg01.u3d",
           "effect/dg02.u3d",
           "effect/dg03.u3d",
           "effect/dg04.u3d",
           "effect/hurt.u3d",
           "effect/skill04.u3d",
           "effect/skill04_debuff.u3d",
           "effect/skill04_hurt.u3d",

            }, doCreateContent);
        } else {
            doCreateContent();
        }
      
    }
   
    protected void doCreateContent() {

        Player pc = CharFactory.createPlayer();
        pc.onBorn();
        addFriend(pc);

        //GameObject playerObject = pc.model;
        //CameraFollowing following = Camera.main.GetComponent<CameraFollowing>();
        //following.target = playerObject.transform;
        //following.quickUpdate();
      
      
       setupAI();
       
        //test 
       FightSkill.instance.loadSkill();

       closeAllDoors();
    }
    public void closeAllDoors() {
        Player.instance.agent.walkableMask = 1 << NavMesh.GetNavMeshLayerFromName("Default");
        foreach (Door d in doorList) {
            d.initFriendsDoorState();
        }
    }
   
    protected void setupAI() {
        PlayerCharacter p = Player.instance;
        p.setAgentEnable(true);

        //foreach (FightCharacter c in friends) {
        //    if (c != p) {
        //        c.setAgentEnable(true);
        //        c.ai.enabled = true;
        //        if (c.ai is PetAI) {
        //            (c.ai as PetAI).petOwner = p;
        //        }
        //    }
        //}

    }
   
    //see BattleUI.summonPet()
    //private void createPets() {
       
    //    PetPosition[] positions = new PetPosition[]{PetPosition.leftforward,PetPosition.rightforward};
    //    for (int i = 0; i < 2; i++) {
    //        PetData data = new PetData();
    //        data.primaryWeaponType = WeaponType.Melee;
    //        data.moveSpeed = PlayerData.instance.moveSpeed * 0.9f;
    //        data.attackSpeed = 0.5f;
    //        data.charTemplate = App.template.chart(5001);//zhanshi
    //        data.hp = data.maxhp = int.MaxValue;
    //        data.team = Player.instance.team.clone();
    //        Pet c = CharFactory.createPet(data, positions[i]);
    //        //c.learnSkill(50, 1);
    //        addFriend(c);
    //    }
    //}
    public Door getDoor(int roomIndex) {
        DungeonTemplate.DungeonRoom prevRoom = dungeonData.template.rooms[roomIndex];
        foreach (Door d in doorList) {
            if (d.room == prevRoom) {
                return d;
            }
        }
        return null;
    }
    public override void prepareRoom() {
        DungeonTemplate.DungeonRoom room = dungeonData.template.rooms[dungeonData.currentRoomIndex];
        if (room.groups.Count < 1) return;
        Team team = new Team();
        team.teamNo = Naming.TeamMonster;
        DungeonTemplate.SpawnGroup group = room.groups[dungeonData.currentGroupIndex];
        if (dungeonData.currentGroupIndex == 0 && dungeonData.currentRoomIndex > 0) {
            Door d = getDoor(dungeonData.currentRoomIndex-1);
            if (d != null) {
                d.close();
            }
           
        }
        dungeonData.totalGroupCount = room.groups.Count;
        int[] monsterIds = group.monsters;         
        if (group.bossID > 0) { //exist boss
            MonsterData data = MonsterData.create(App.template, group.bossID);
            data.team = team.clone();
            Monster c = CharFactory.createMonster(data);
            //Vector2 random2d = Random.insideUnitCircle * group.radius;
            Vector3 position = new Vector3(group.bossx, group.y, group.bossz);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 100, 1)) {
                position = hit.position;
            }
            c.transform.position = position;
            addEnemy(c);
            BossBorn bb = c.model.AddComponent<BossBorn>();
            bb.onBegin(monsterIds, group, team);
            bb.callBack = bossBornComplete;
            boss = c;
            App.sceneManager.changeBgSound("BOSS_bg");
        }
        else {
            spawnMonster(monsterIds, group, team);
        }
    }

    private int[] script_monsterIds;
    private DungeonTemplate.SpawnGroup script_group;
    private Team script_team;
    private void bossBornComplete(int[] monsterIds, DungeonTemplate.SpawnGroup group, Team team)
    {
        this.script_monsterIds = monsterIds;
        this.script_group = group;
        this.script_team = team;
        ScriptManager.instance.onComplete = scriptComplete;
        ScriptManager.instance.trigger(2);
        //BossHead.instance.reset(boss);
    }
    private void scriptComplete() {
        spawnMonster(script_monsterIds, script_group, script_team);
        if(boss!=null)
            BossHead.instance.reset(boss);
    }

    private void spawnMonster(int[] monsterIds, DungeonTemplate.SpawnGroup group, Team team) {
        int length =  monsterIds.Length;
        Templates templates = App.template;
        Vector3 tempPosition = Vector3.zero;
        bool hasBoss = false;
        float delay = 0;
        int playerAgentWalkableMask = Player.instance.agent.walkableMask;
        for (int i = 0; i < length; i += 2) {
            int id = monsterIds[i];
            if (id == group.bossID) continue;
            int count = monsterIds[i + 1];
            for (int j = 0; j < count; j++) {
                MonsterData data = MonsterData.create(templates, id);
                data.team = team.clone();
                Monster c = CharFactory.createMonster(data);
                c.model.SetActive(false);
                c.delay = delay;
                delay += 0.5f;
                //random position
                //by default,monster is in obstacle mode,so we can random a position correctly
                Vector2 random2d = Random.insideUnitCircle * group.radius;
                tempPosition.Set(group.x + random2d.x, group.y, group.z + random2d.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(tempPosition, out hit, 100, 1)) {
                    tempPosition = hit.position;
                }
                c.transform.position = tempPosition;
                addEnemy(c);

                c.agent.walkableMask = playerAgentWalkableMask;
            }
            delay += 1f;
            Debug.Log("monsters:" + enemies.Count);
        }

        if (hasBoss)
        {

            ScriptManager.instance.trigger(2);
        }
        //foreach (FightCharacter c in enemies) {
        //     c.ai.enabled = true;
        //     c.setObstacleMode(false);
        //}
    }


    protected override bool nextGroup() {
        
        if (!base.nextGroup()) {
            if (dungeonData.hasNextRoom()) {
                specialMonsters.Clear();
                int roomIndex = dungeonData.currentRoomIndex + 1;
                if (!dungeonData.roomStates[roomIndex].doorOpened) {
                    DungeonTemplate.DungeonRoom room = dungeonData.template.rooms[roomIndex];
                    foreach (Door d in doorList) {
                        if (d.room == room) {
                            dungeonData.roomStates[roomIndex].doorOpened = true;
                            Player.instance.forceIdle();
                            App.coroutine.StartCoroutine(openDoor(d));

                            break;
                        }
                    }
                }
            }
        } else {
            foreach (Monster m in specialMonsters) enemies.Add(m);
        }
        return false;
    }
    IEnumerator openDoor(Door d) {
        yield return new WaitForSeconds(2);
        App.sceneManager.onSceneTrigger(null,d);
    }

    public Vector3 getNextRoomPosition() { 
        if(dungeonData.currentRoomIndex>=dungeonData.template.rooms.Length-1) return Vector3.zero; 
        DungeonTemplate.DungeonRoom room = dungeonData.template.rooms[dungeonData.currentRoomIndex+1];
        if (room.groups.Count < 1) return Vector3.zero;
        DungeonTemplate.SpawnGroup group = room.groups[0];
        Vector3 position = new Vector3(group.x, group.y, group.z);
        return position;
    }
    private List<Monster> specialMonsters = new List<Monster>();
    public void spawnSpecialMonster(int charTempalteId) {
        CharTemplate template=App.template.getTemp<CharTemplate>(charTempalteId);
        MonsterData data = MonsterData.create(App.template, charTempalteId);
        Team team = new Team();
        team.teamNo = Naming.TeamMonster;
        data.team = team;
        Monster c = CharFactory.createSpecialMonster(data);
        c.deadable = false;
        c.model.SetActive(true);
      
        DungeonTemplate.DungeonRoom room = dungeonData.template.rooms[dungeonData.currentRoomIndex];
        DungeonTemplate.SpawnGroup group = room.groups[dungeonData.currentGroupIndex];
        Vector2 random2d = Random.insideUnitCircle * group.radius;
        Vector3 tempPosition = new Vector3(group.x + random2d.x, group.y, group.z + random2d.y);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(tempPosition, out hit, 100, 1)) {
            tempPosition = hit.position;
        }
        c.transform.position = tempPosition;
        addEnemy(c);
        specialMonsters.Add(c);
        c.agent.walkableMask = Player.instance.agent.walkableMask;
    }

}
