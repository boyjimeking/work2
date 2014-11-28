using UnityEngine;
using System.Collections;
namespace engine {
    //hero or other player
    public class PlayerCharacter : FightCharacter {

        public override void reset(GameObject model, CharData data, AI ai)
        {
            base.reset(model, data, ai);
            HeadController.instance.reset(data);
        }

        public override void applyDamage(AttackResult attackResult, FightCharacter attacker)
        {
            base.applyDamage(attackResult, attacker);
            //HeadController.instance.setHP(data.maxhp, data.hp);
        }


    }
}

