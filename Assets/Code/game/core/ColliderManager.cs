using UnityEngine;
using System.Collections;
using engine;

public class ColliderManager : IColliderManager {
    //since collision occurs mutually,we only deal with the case that A is FighterCharacter.
     public void onTriggerEnter(Binding A, Collider other, bool isUpdate){
        Binding B = other.gameObject.GetComponent<Binding>();
        if (B == null || B.data == null || A == B || A.data == B.data) return;

        if (A.data == Player.instance) {
            if (B.data is SceneTrigger) {
                App.sceneManager.onSceneTrigger(Player.instance, B.data as SceneTrigger);
                return;
            }
            if (B.data is DropItem) {
                DropManager.instance.pickupDrop(B.data as DropItem);
                return;
            }
        } else if (A.data is Pet) {
            if (B.data is SceneTrigger) {
                App.sceneManager.onSceneTrigger(A.data as Pet,B.data as SceneTrigger);
                return;
            }
        }

        if (!(A.data is FightCharacter)) {
            if (A.data is BoomBox)
            {
                FightCharacter owner = null;
                if (B.data is engine.Bullet) owner = (B.data as engine.Bullet).owner;
                else if (B.data is Weapon) owner = (B.data as Weapon).owner;
                else if (B.data is ColliderObject) owner = (B.data as ColliderObject).owner;
                if (owner != Player.instance) return;
                BoomBoxManager.instance.destroy((BoomBox)A.data);
            }
            else if (A.data is ExploderManager.ExploderOptions) {
                FightCharacter owner = null;
                if (B.data is engine.Bullet) owner = (B.data as engine.Bullet).owner;
                else if (B.data is Weapon) owner = (B.data as Weapon).owner;
                else if (B.data is ColliderObject) owner = (B.data as ColliderObject).owner;
                if (owner != Player.instance) return;
                ExploderManager.ExploderOptions ee = A.data as ExploderManager.ExploderOptions;
                if (ee.explodered) return;
                ee.explodered = true;
                ExploderManager.instance.exploder(A.gameObject, ee);
                
            }
            return;
        } 
        
        FightCharacter fighterA = A.data as FightCharacter;
        if (fighterA.isDead()) return;
        if (B.data is Bullet) {
            Bullet bullet = B.data as Bullet;
            if (bullet.owner == fighterA) return;
            if (BattleEngine.scene.isDifferentTeam(bullet.owner, fighterA)) {
                SkillEffectTemplate temp = bullet.temp;
                if (isUpdate && !fighterA.existMultiSKill(temp.id)) {
                    return;
                }
                fighterA.takeDamage(bullet, bullet.owner.currentSkillEffect.hitEffect);
                if (bullet.temp.bulletmode == 1)
                {
                    bullet.destroy();
                }
                if (!isUpdate) {
                    if (temp.hurtNum > 1) {
                        fighterA.addSkillState(bullet.owner, temp);
                    }
                }
                if (bullet.owner == Player.instance)
                {
                    Combo.instance.playHit();
                }
            }

        } else if (B.data is Weapon) {
            Weapon weapon = B.data as Weapon;
            FightCharacter fighterB = weapon.owner;
            if (fighterA == fighterB) return;
            if (fighterA.globalHitNo != BattleEngine.atkNo && fighterA.localHitNo != fighterB.atkNo) {
                fighterA.globalHitNo = BattleEngine.atkNo;
                fighterA.localHitNo = fighterB.atkNo;
                if (BattleEngine.scene.isDifferentTeam(fighterB, fighterA)) {
                    if (isUpdate && !fighterA.existMultiSKill(fighterB.currentSkillEffect.id)) {
                        return;
                    }
                    fighterA.takeDamage(fighterB);
                    if (fighterB == Player.instance) {
                        App.effect.shakeCamera(0.3f, 0.5f);
                        Combo.instance.playHit();
                        if (!fighterA.isBoss() || !fighterA.isDead())
                            App.effect.checkSlowTimeScale();
                    }
                }
            }

        } else if (B.data is ColliderObject) {
            ColliderObject co = B.data as ColliderObject;
            if (co.owner == fighterA) return;
            if (BattleEngine.scene.isDifferentTeam(co.owner, fighterA)) {
                if (fighterA.addHit(co)) {
                    if (isUpdate && !fighterA.existMultiSKill(co.effect.id)) {
                        return;
                    }
                    co.addHit(fighterA);

                    fighterA.takeDamage(co, co.effect.hitEffect, co.resName == "man_skill1_la");

                    if (co.owner == Player.instance)
                    {
                        Combo.instance.playHit();
                    }

                    if (co.resName == "man_skill1_la")//拉怪技能
                    {
                        if (!fighterA.isDead()) App.coroutine.StartCoroutine(laguai(fighterA, co.owner.model.transform));
                    } else {
                        //主角普攻震屏效果
                        if (co.owner == Player.instance) {
                            App.effect.shakeCamera(0.3f, 0.5f);
                            if (!fighterA.isBoss() || !fighterA.isDead())
                                App.effect.checkSlowTimeScale();
                        }
                    }
                }
            }
            
        }
    }
     public IEnumerator laguai(FightCharacter fighter, Transform _transform)
     {
         fighter.ai.enabled = false;
         fighter.agentStop();
         fighter.setObstacleMode(false);
         fighter.controller.setTrigger(Hash.idleState);
         float distance = Vector3.Distance(fighter.model.transform.position, _transform.position);
         Vector3 targetPosition = Vector3.MoveTowards(fighter.model.transform.position, _transform.position, distance - 1.1f);
         iTween.MoveTo(fighter.model, iTween.Hash("x", targetPosition.x, "z", targetPosition.z, "easeType", iTween.EaseType.easeInExpo, "time", 0.1f));
         yield return new WaitForSeconds(0.15f);
         fighter.ai.enabled = true;
     }
    //private IEnumerator calResult(FightCharacter c, Bullet b, int num, float t) {
    //    for (int i = 0; i < num; i++) {
    //        c.takeDamage(b, b.owner.currentSkillEffect.hitEffect);
    //        b.destroy();
    //        if(i == num-1)
    //            b.destroy();
    //        yield return t;
    //    }
    //}
}
