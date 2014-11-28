using UnityEngine;
using System.Collections;
namespace engine{
    //gobal battle flow decision maker.
    public interface IAttackController {
      
        AttackResult calcDamage(FightCharacter attacker, IAttackable target, bool canEVA = true);
        bool canLaunchSkill(FightCharacter attacker, LearnedSkill skill);

        //check the melee attack validity.
        //this is called after attacker and target have collided each other.
        bool isValidMeleeHit(FightCharacter attacker, GameObject target);

        HitState calcHitState(FightCharacter attacker, FightCharacter attackee, SkillEffectTemplate skill);
    }
}

