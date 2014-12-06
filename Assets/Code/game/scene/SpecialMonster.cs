using UnityEngine;
using System.Collections;
using engine;

//demo用特殊怪，不掉血，可击倒击退，会flee，距离玩家保持一段距离
//@see SpecialMonsterAI
public class SpecialMonster : Monster {
    public override void reset(GameObject model, CharData data, AI ai) {
        base.reset(model, data, ai);
        agent.radius = 0.05f;
        agent.angularSpeed = 100000;
    }
    public override void applyDamage(AttackResult attackResult, FightCharacter attacker) {
        this.attacker = attacker;
        //do nothing
    }
    
}
