using UnityEngine;
using System.Collections;

namespace engine {
    public class DefaultAttackController:IAttackController {

        public AttackResult calcDamage(FightCharacter attacker, IAttackable target, bool canEVA=true)
        {
            Stats a=attacker.stats;
            Stats b=(target as FightCharacter).stats;
            //伤害结果
            AttackResult attackResult = new AttackResult(0);
            bool isEVA = canEVA && Random.Range(0f, 1f) <= b.EVA;
            if (isEVA)//触发闪避
            {
                attackResult.state = AttackState.eva;
            }
            else//不闪避
            {
                int damage = Mathf.Max((int)((a.ATT - b.DEF) * Random.Range(0.95f, 1.05f)), 1);
                bool isCRI = Random.Range(0f, 1f) <= a.CRI;
                if (isCRI)//暴击
                {
                    damage *= 2;
                    attackResult.state = AttackState.critical;
                }
                attackResult.damage = damage;
            }
            return attackResult;
        }
        public bool canLaunchSkill(FightCharacter attacker, LearnedSkill skill) {
            return true;
        }
       

        public bool isValidMeleeHit(FightCharacter attacker, GameObject hitObject) {
            GameObject go = attacker.model;
            Transform attackerTransform = attacker.transform;
            
            if (hitObject == go || hitObject.layer!=go.layer) return false;
            Vector3 dir = (hitObject.transform.position - attackerTransform.position).normalized;
            float direction = Vector3.Dot(dir, attackerTransform.forward);
            if (direction < 0.5f) return false;
            return true;

        }

        protected HitState hitState = new HitState(); 
        //NOTE:the return value is shared.
        public HitState calcHitState(FightCharacter attacker, FightCharacter target, SkillEffectTemplate skill) {
            hitState.attacker = attacker;
            Stats stats=target.stats;
            hitState.hitback = Utility.calSuccess(skill.knockback[0] - stats.BeatBack); 
            hitState.hitbackDistance = skill.knockback[1]*stats.BackDistance;
            hitState.hitdown = Utility.calSuccess(skill.knockdown - stats.BeDown);
            hitState.playHit = Utility.calSuccess(skill.playHit - stats.BeHit);         
            return hitState;
        }
    }

}
