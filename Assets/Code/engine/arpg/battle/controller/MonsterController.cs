using UnityEngine;
using System.Collections;
namespace engine {
    public class MonsterController : AvatarController {
        public override  void resetAttackTrigger() {
            animator.ResetTrigger(Hash.monsterAtk1State);
        }
    }
}

