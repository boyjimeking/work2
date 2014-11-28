using UnityEngine;
using System.Collections;
namespace engine {
    public class PetCharacter : FightCharacter {
        public override  bool inAttackAnimation() {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            int state = info.nameHash;
            return state == Hash.monsterAtk1State || state == Hash.skill01State;
        }
    }
}

