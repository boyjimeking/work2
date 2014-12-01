using UnityEngine;
using System.Collections;
using engine;
using System;

//help class to create various model object.
public class CharFactory  {
    public static Player createPlayer() {
        Player c = Player.instance;

        PlayerData data = PlayerData.instance;
        data.init();
        //data.hp = data.maxhp = int.MaxValue;

        GameObject player = App.res.createSingle("Local/prefab/avatar/man");
        
        //idle weapon
        GameObject weapon = App.res.createSingle("Local/prefab/weapon/ldweapon");// App.bundle.create("weapon/ld.u3d", "ld");
        Transform hand = CharHelper.getBodyPart(player, "jianshi", "right_hand");
        resetParent(weapon, hand);
        weapon.name = "weapon";
        Weapon idle = new Weapon(c, weapon, WeaponType.Melee);

        //atk weapon
        weapon = App.res.createSingle("Local/prefab/weapon/ldweapon_atk");
        Weapon atkWeapon = new Weapon(c, weapon, WeaponType.Melee);
        resetParent(weapon, hand);
        weapon.SetActive(false);//hide atk weapon

        c.setWeapons(idle, atkWeapon, idle);
        
        c.reset(player, data, null);


        //player host for attack rush effect
        
        //GameObject ghostObject = App.res.createSingle("Local/prefab/avatar/man");
        //GameObject ghostWeapon = App.res.createSingle("Local/prefab/weapon/ldweapon_atk");
        //hand = CharHelper.getBodyPart(ghostObject, "jianshi", "right_hand");
        //resetParent(ghostWeapon, hand);
        PlayerGhost ghost = PlayerGhost.instance;
        //ghost.reset(ghostObject,ghostWeapon);
        ghost.reset();


        //GameObject swordEffectPrefab= Engine.res.loadPrefab("prefab/effect/sword_effect2");
        //GameObject effect = GameObject.Instantiate(swordEffectPrefab) as GameObject;
        //effect.transform.parent = weapon.transform;
        //effect.resetLocalTransform();
        //effect.transform.localPosition = new Vector3(0, 0, 0.5f);
        //c.weapon.effect = effect;


        //GameObject namePrefab = Engine.res.loadPrefab("Local/prefab/charName");
        //GameObject name = GameObject.Instantiate(namePrefab) as GameObject;
        //name.name = "charName";
        //name.transform.parent = player.transform;
        //name.transform.localPosition = new Vector3(0, 1.1f, 0);
        //TextMesh text = name.GetComponent<TextMesh>();
        //text.text = "玩家名字";
        //c.charName = name;
        //name.SetActive(false);


        //normal attack
        c.learnSkill(1, 1);
        c.learnSkill(2, 1);
        c.learnSkill(3, 1);
        c.learnSkill(4, 1);
        c.learnSkill(50, 1);

        //skill
        //c.learnSkill(50, 1);

        //bind skill to attack button
        //App.input.bindAttackButton(Naming.AttackButton1, c.getLearnedSkill(50));

        return c;
    }
    public static Monster createMonster(MonsterData data) {
        GameObject model = App.res.createSingle("Local/prefab/avatar/"+data.charTemplate.model);// App.bundle.create("avatar/" + data.charTemplate.model + ".u3d", data.charTemplate.model);
        Monster c = new Monster();
        MonsterAI ai= new MonsterAI();
//        ai.path = "Local/ai2/monster";
        c.reset(model, data, ai);

        Renderer r = model.GetComponentInChildren<Renderer>();
        if (r != null)
        {
            RenderVisible renderVisible = r.gameObject.addOnce<RenderVisible>();
            renderVisible.c = c;
        }
        return c;
    }
    public static Monster createSpecialMonster(MonsterData data) {
        GameObject model = App.res.createSingle("Local/prefab/avatar/" + data.charTemplate.model);// App.bundle.create("avatar/" + data.charTemplate.model + ".u3d", data.charTemplate.model);
        Monster c = new SpecialMonster();
        MonsterAI ai = new SpecialMonsterAI();
        //        ai.path = "Local/ai2/monster";
        c.reset(model, data, ai);

        Renderer r = model.GetComponentInChildren<Renderer>();
        if (r != null) {
            RenderVisible renderVisible = r.gameObject.addOnce<RenderVisible>();
            renderVisible.c = c;
        }
        return c;
    }
    public static Pet createPet(PetData data,PetPosition position,int walkablemask) {
        Pet c = new Pet();

        GameObject go = App.res.createSingle("Local/prefab/avatar/" + data.charTemplate.model);// App.bundle.create("avatar/" + data.charTemplate.model + ".u3d", data.charTemplate.model);
        //go.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        
        PetAI ai = new PetAI(position);
     //   ai.path = "Local/ai2/pet";
        c.reset(go, data, ai);

        //assign pet position
        c.transform.position = ai.getPosition(Player.instance.transform,walkablemask);

        foreach (int skillId in data.charTemplate.skills)
        {
            c.learnSkill(skillId, 1);
        }
        return c;
    }
    public static OtherPlayer createOtherPlayer(OtherPlayerData data) {
        return null;
        //OtherPlayer c = new OtherPlayer();
        ////TODO create from data
        //GameObject model = App.bundle.create("avatar/jianshi.u3d", "jianshi");

        //GameObject weapon = App.bundle.create("weapon/sword.u3d", "sword");
        //Transform hand = CharHelper.getBodyPart(model, "jianshi", "right_hand");
        //resetParent(weapon, hand);
        //weapon.name = "weapon";
        //Collider collider = weapon.GetComponent<Collider>();
        //if (collider == null) {
        //    collider = weapon.AddComponent<BoxCollider>();
        //}
        //collider.isTrigger = true;
        //collider.enabled = false;
        //weapon.addOnce<Binding>().data = c.weapon = new Weapon(c, weapon, WeaponType.Melee);

        //c.reset(model, data, null);
        //return c;
    }
    private static void resetParent(GameObject child, Transform newParent) {
        child.transform.parent = newParent;
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;
    }
}
