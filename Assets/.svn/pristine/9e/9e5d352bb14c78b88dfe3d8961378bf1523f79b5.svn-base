using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace engine {
    public class MonsterAI : AI {

        protected bool attackImmediately;
        protected float nohittime;
        protected static float CHANGETARGETCD = 5f;//如果5s都没命中 则切换目标
       
        public void resetNoHit()
        {
            nohittime = 0;
        }
        public override void update() {
            if (!enabled) return;
            if (owner.isDead()||owner.isHitDown||owner.isHitBack||owner.isPlayHit||owner.isAttacking||owner.isFreezing) return;
           
            if (target != null && !target.isDead()) {
                if (owner.inAttackRange(target,owner.data.weaponRange)) {
                    owner.agentStop();
                    owner.setObstacleMode(true);
                    if (!owner.inAttackAnimation()) {
                        owner.lookat(target);
                    }
                     if (tryAttack()) {

                     }
                     checkChangeTarget();
                } else if (owner.inMoveRange(target)) {
                    attackTimer = attackInterval;
                    if (!owner.agent.enabled) {
                        owner.setObstacleMode(false);
                        return;//give unity some time to turn off obstacle/carving.
                    }
                    if (steering) {
                        owner.agentMoveTo(steeringPosition);
                    } else {
                        AnimatorStateInfo info = owner.animator.GetCurrentAnimatorStateInfo(0);
                        if (info.nameHash != Hash.skill02State) {
                            owner.agentMoveTo(target);
                            checkChangeTarget();
                        }
                    }
                   
                } else {
                    target = null;
                    attackTimer = attackInterval;
                }
            } else {
                resetNoHit();
                owner.agentStop();
                owner.setObstacleMode(true);
                target = BattleEngine.scene.findNearestTarget(owner);
            }
        }
        

        public void checkChangeTarget()
        {
            nohittime += Time.deltaTime;
            if (nohittime > CHANGETARGETCD)
            {
                resetNoHit();
                target = BattleEngine.scene.findNearestTarget(owner, target);
            }
        }
    }
}

