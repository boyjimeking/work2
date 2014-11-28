using System;
using UnityEngine;
using System.Collections;
using engine;
using Object = UnityEngine.Object;

public class AnimationManager : IAnimEventManager {
    public void handle(Binding binding, AnimationEvent ev) {
        int type = ev.intParameter;
        string param = ev.stringParameter;
        
        switch (type) {
            case 1: playEffect(binding, param); break;//播放特效
            case 2: genCollider(binding, param); break;//产生碰撞盒
            case 3: playAudio(binding, param); break;//播放声音
            case 4: playBullet(binding, param); break;//播放子弹
            case 5: makeDamageBegin(binding, param); break;//伤害帧开始
            case 6: makeDamageEnd(binding, param); break;//伤害帧结束
            case 7: comboPhaseBegin(binding, param); break;//连击开始
            case 8: playLocalEffect(binding, param); break;
            case 9: changeTarget(binding); break;
            case 100: //播放胜利动画
                playWin();
                break;
            case 111: //playSummon effect
                playSummon(binding);
                break;
            case 112: //begin skill result
                playSkill(binding, param);              
                break;
            case 114://怪物能够被攻击
                canTriger(binding);              
                break;
        }
    }
    private void playLocalEffect(Binding c, string param) {
        string[] value=param.Split(',');
        string effectName=null, bodyPartName=null;
        if (value.Length == 2) {
            effectName = value[0].Trim();
            bodyPartName = value[1].Trim();
        } else {
            effectName = param;
        }
        FightCharacter f = c.data as FightCharacter;
        GameObject fxEff = App.res.createSingle("Local/prefab/effect/" + effectName);
        fxEff.SetActive(false);
        Transform parent = null;
        if (bodyPartName != null) {
            parent = CharHelper.getBodyPart(f.model, f.charTemplate.name, bodyPartName);
        } 
        if(parent==null){
             parent = f.transform;
        }
        
        fxEff.transform.parent = parent;
        fxEff.resetLocalTransform();
        fxEff.SetActive(true);
        GameObject.Destroy(fxEff, 2f);   
    }
    protected void makeDamageBegin(Binding c, string resName) {
        FightCharacter p = c.data as FightCharacter;
        if (p == null) return;
        p.enableWeaponCollider(true);
        BattleEngine.atkNo++;
        p.atkNo++;
        
    }
    protected void makeDamageEnd(Binding c, string resName) {
        FightCharacter p = c.data as FightCharacter;
        if (p == null) return;
        p.enableWeaponCollider(false);
        
    }
    protected void comboPhaseBegin(Binding c, string resName) {
      //  c.comboPhaseBegin();
        Player p = c.data as Player;
        if (p == null) return;
        p.startReceiveCombo();
    }

    public void playEffect(Binding c, string resName) {
        FightCharacter f = c.data as FightCharacter;
        if (resName.Contains(",")) {
            string[] effects = Utility.toArray(resName);
            GameObject effect = App.res.createSingle("Local/prefab/effect/" + effects[0]);
            if (effect == null) {
                Debug.Log("can't load effect:" + effects[0]);
                return;
            }
            float t = Utility.toFloat(effects[1]);
            if (t == 0) t = 1f;
            effect.SetActive(false);
            effect.transform.position = f.transform.position;
            effect.transform.forward = f.transform.forward;
            f.effectBySelf = effect;
            effect.SetActive(true);
            Object.Destroy(effect, t);
            f.controller.useAnimatorPosition = true;
            return;
        }
        GameObject fxEff = App.res.createSingle("Local/prefab/effect/" + resName);
        if (fxEff == null) {
            Debug.Log("can load effect:" + resName);
            return;
        }
        fxEff.SetActive(false); 
        if(f.currentSkillEffect!=null){
            int id=f.currentSkillEffect.id;
            if (id == 1 || id == 2 || id == 3 || id == 4) {
                //player normal attack,append effect to player
                //TODO player action and effect animation event callback may be out of sync,we need a better way.
                fxEff.transform.parent = f.transform;
                fxEff.resetLocalTransform();
            } else {
                fxEff.transform.position = f.transform.position;
                fxEff.transform.forward = f.transform.forward;
            }
        }else{
            fxEff.transform.parent = f.transform;
            fxEff.resetLocalTransform();
        }
        fxEff.SetActive(true);
        Object.Destroy(fxEff, 1f);
    }

    public void changeTarget(Binding binding) {
        FightCharacter self = binding.data as FightCharacter;
        if (self.AttackTarget != null) {
            Vector3 vec = self.AttackTarget.Position;
            if (self.effectBySelf != null) {
                self.effectBySelf.transform.position = vec;
            }
            vec.y = self.Position.y;
            self.transform.position = vec;
        }
    }

    protected void playAudio(Binding c, string resName) {
        Character f=c.data as Character;
        AudioSource.PlayClipAtPoint(Engine.res.loadSound(Naming.SoundPath+resName), f.transform.position);
    }
    protected void playBullet(Binding c, string resName) {
        int id = Utility.toInt(resName);
        SkillEffectTemplate temp = App.template.getTemp<SkillEffectTemplate>(id);
        if(temp == null)
            return;
        FightCharacter actor = c.data as FightCharacter;
        if (actor.currentSkill == null) return;
        Bullet bullet =  new engine.Bullet();    
        bullet.reset(temp, actor);
        BattleEngine.scene.addBullet(bullet);
       
    }
    protected void genCollider(Binding c, string resName, float life=0.1f, FightCharacter target=null) {

        GameObject prefab = Engine.res.loadPrefab(Naming.ColliderPath + resName);
        if (prefab == null) return;

        GameObject colliderObject = GameObject.Instantiate(prefab) as GameObject;
        //TODO make this in prefab
        MeshRenderer render = colliderObject.GetComponentInChildren<MeshRenderer>();
        render.enabled = false;

        colliderObject.resetLocalTransform();
        if (target != null) {
            colliderObject.transform.position = target.transform.position;
            colliderObject.transform.eulerAngles = target.transform.eulerAngles;
        }
        else {
            colliderObject.transform.position = c.transform.position;
            colliderObject.transform.eulerAngles = c.transform.eulerAngles;
        }       

        ColliderObject co = BattleEngine.getColliderObject();

        Collider[] colliders = colliderObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders) {
            collider.isTrigger = true;
            collider.gameObject.addOnce<Binding>().data = co;
        }

        FightCharacter f = c.data as FightCharacter;
        co.reset(f, f.currentSkill, f.currentSkillEffect, resName);

        colliderObject.addOnce<DestroyListener>().reset(co, (data) => {
            (data as ColliderObject).onDestroy();
        });
        //Object.Destroy(colliderObject, 0.1f); 
        Object.Destroy(colliderObject, life);     
    }

    private void playWin() {
        //if (BattleEngine.scene.getFriends().Count > 1) {
        //    foreach (FightCharacter c in BattleEngine.scene.getFriends()) {
        //        if (!c.isPlayer()) {
        //            c.ai.enabled = false; //disable player, pet AI action
        //            c.setAgentEnable(false);
        //        }
        //    }
        //}
        //GameObject obj = new GameObject();
        //MonoBehaviour mono = obj.AddComponent<MonoBehaviour>();
        //mono.StartCoroutine(win(obj));
    }

    private IEnumerator win(GameObject obj) {
        yield return new WaitForSeconds(2f);
        BattleWin win = Player.instance.model.AddComponent<BattleWin>();
        win.onBegin();
        Object.Destroy(obj);
    }

    private void playSummon(Binding binding) {
        //Pet c = binding.data as Pet;
        //c.transform.position = Player.instance.Position;
        //GameObject effect = Engine.res.createSingle("Local/prefab/effect/" + c.charTemplate.summonEffect);
        //effect.transform.position = c.transform.position;
        //CameraManager.focusObj(c.model, effect);
    }

    private void playSkill(Binding binding, string effect) {
        int id = Utility.toInt(effect);
        SkillEffectTemplate temp = App.template.getTemp<SkillEffectTemplate>(id);
        if (temp != null) {
            //if (temp.hurtNum > 1 && temp.lastTime > 0 && temp.colliderType != ColliderType.bullet) {
            //    float t = temp.lastTime/temp.hurtNum;
            //    App.coroutine.StartCoroutine(calResult(temp, binding, t, temp.hurtNum));
            //}
            //else {
            //    App.coroutine.StartCoroutine(calResult(temp, binding, 0, 1));
            //}
            calResult(temp, binding);
        }
    }

    private void canTriger(Binding b)
    {
        Monster m = b.data as Monster;
        if (m == null || m.cc==null) return;
        m.cc.enabled = true;
    }
    //private void calResult(SkillEffectTemplate temp, Binding binding, float time, int num) {
    private void calResult(SkillEffectTemplate temp, Binding binding) {
        //for (int i = 0; i < num; i++) {
            FightCharacter owner = binding.data as FightCharacter;
            switch (temp.colliderType) {
                case ColliderType.none:
                    break;
                case ColliderType.effect:
                    break;
                case ColliderType.collider:
                    genCollider(binding, temp.collider, temp.colliderLife);
                    break;
                case ColliderType.bullet:
                    playBullet(binding, temp.id.ToString());
                    break;
                case ColliderType.circle:
                    foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
                        if (!c.isDead() && BattleEngine.scene.judgeInRange(c.transform, owner.transform, temp.radius)) {
                            c.takeDamage(owner, temp);
                        }
                    }
                    break;
                case ColliderType.sector:
                    foreach (FightCharacter c in BattleEngine.scene.getEnemies()) {
                        if (!c.isDead() &&
                            BattleEngine.scene.judgeInRange(c.transform, owner.transform, temp.radius, temp.angle)) {
                            c.takeDamage(owner, temp);
                        }
                    }
                    break;
                case ColliderType.colliderTarget:
                    FightCharacter self = binding.data as FightCharacter;
                    if (self != null)
                        genCollider(binding, temp.collider, temp.colliderLife, self.AttackTarget);
                    break;
                default:
                    Debug.LogError("collider type is wrong config!!!!!");
                    break;
            }
            //yield return time;
        //}
    }
}
